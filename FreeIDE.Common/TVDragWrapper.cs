using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Microsoft.VisualBasic; // Install-Package Microsoft.VisualBasic
using static FreeIDE.Common.ShellDll;

namespace FreeIDE.Common
{
    public partial class TVDragWrapper : ShellDll.IDropTarget, IDisposable
    {
        private Control m_View;
        private int m_Original_Effect; // Save it
        private int m_OriginalRefCount; // Set in DragEnter, used in DragDrop
        private IntPtr m_DragDataObj; // Saved on DragEnter for use in DragOver
        private ShellDll.IDropTarget m_LastTarget; // Of most recent Folder dragged over
        private object m_LastNode; // Most recent node dragged over
        private ArrayList m_DropList; // CShItems of Items dragged/dropped
        private CProcDataObject m_MyDataObject; // Does parsing of dragged IDataObject

        public event ShDragEnterEventHandler ShDragEnter;
        public delegate void ShDragEnterEventHandler(ArrayList DragItemList, IntPtr pDataObj, int grfKeyState, int pdwEffect);

        public event ShDragOverEventHandler ShDragOver;
        public delegate void ShDragOverEventHandler(object Node, System.Drawing.Point ClientPoint, int grfKeyState, int pdwEffect);

        public event ShDragLeaveEventHandler ShDragLeave;
        public delegate void ShDragLeaveEventHandler();

        public event ShDragDropEventHandler ShDragDrop;
        public delegate void ShDragDropEventHandler(ArrayList DragItemList, object Node, int grfKeyState, int pdwEffect);

        public TVDragWrapper(TreeView ctl) => m_View = ctl; 

        private bool disposedValue = false; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: free managed resources when explicitly called
                }
                // TODO: free shared unmanaged resources
                ResetPrevTarget();
            }
            disposedValue = true;
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void ResetPrevTarget()
        {
            if (!(m_LastTarget == null))
            {
                int hr = m_LastTarget.DragLeave();
                Marshal.ReleaseComObject(m_LastTarget);
                m_LastTarget = null;
                m_LastNode = null;
            }
        }

        public int DragEnter(IntPtr pDataObj, int grfKeyState, POINT pt, ref int pdwEffect)
        {
            Debug.WriteLine("In DragEnter: Effect = " + pdwEffect + " Keystate = " + grfKeyState);
            m_Original_Effect = pdwEffect;
            m_DragDataObj = pDataObj;
            m_OriginalRefCount = Marshal.AddRef(m_DragDataObj); // note: includes our count
            Debug.WriteLine("DragEnter: pDataObj RefCnt = " + m_OriginalRefCount);

            m_MyDataObject = new CProcDataObject(ref pDataObj);

            if (m_MyDataObject.IsValid)
            {
                m_DropList = m_MyDataObject.DragList;
                ShDragEnter?.Invoke(m_DropList, pDataObj, grfKeyState, pdwEffect);
            }
            else
            {
                pdwEffect = (int)DragDropEffects.None;
            }
            return 0;
        }

        public int DragOver(int grfKeyState, POINT pt, ref int pdwEffect)
        {
            object tn;
            System.Drawing.Point ptClient = m_View.PointToClient(new System.Drawing.Point(pt.x, pt.y));
            tn = ((TreeView)m_View).GetNodeAt(ptClient);
            if (tn == null) // not over a TreeNode
            {
                ResetPrevTarget();
            }
            else // currently over Treenode
            {
                if (!(m_LastNode == null)) // not the first, check if same
                {
                    if (ReferenceEquals(tn, m_LastNode))
                    {
                        return 0; // all set up anyhow
                    }
                    else
                    {
                        ResetPrevTarget();
                        m_LastNode = tn;
                    }
                }
                else // is the first
                {
                    ResetPrevTarget(); // just in case
                    m_LastNode = tn;
                } // save current node

                // Drag is now over a new node with new capabilities
                CShItem CSI = ((tn as TreeNode).Tag as CShItem);
                if (CSI.IsDropTarget)
                {
                    m_LastTarget = CSI.GetDropTargetOf(m_View);
                    if (!(m_LastTarget == null))
                    {
                        pdwEffect = m_Original_Effect;
                        int res = m_LastTarget.DragEnter(m_DragDataObj, grfKeyState, pt, pdwEffect);
                        if (res == 0)
                        {
                            res = m_LastTarget.DragOver(grfKeyState, pt, pdwEffect);
                        }
                        if (res != 0)
                        {
                            Marshal.ThrowExceptionForHR(res);
                        }
                    }
                    else
                    {
                        pdwEffect = 0;
                    } // couldn't get IDropTarget, so report effect None
                }
                else
                {
                    pdwEffect = 0;
                }   // CSI not a drop target, so report effect None
                ShDragOver?.Invoke(tn, ptClient, grfKeyState, pdwEffect);
            }
            return 0;
        }

        public int DragLeave()
        {
            m_Original_Effect = 0;
            ResetPrevTarget();
            int cnt = Marshal.Release(m_DragDataObj);
            Debug.WriteLine("DragLeave: cnt = " + cnt);
            m_DragDataObj = IntPtr.Zero;
            m_OriginalRefCount = 0;      // just in case
            m_MyDataObject = null;
            ShDragLeave?.Invoke();
            return 0;
        }

        public int DragDrop(IntPtr pDataObj, int grfKeyState, POINT pt, ref int pdwEffect)
        {
            int res;
            if (!(m_LastTarget == null))
            {
                res = m_LastTarget.DragDrop(pDataObj, grfKeyState, pt, pdwEffect);
                // version 21 change 
                if (res != 0 && res != 1)
                {
                    Debug.WriteLine("Error in dropping on DropTarget. res = " + Conversion.Hex(res));
                } // No error on drop
                  // it is quite possible that the actual Drop has not completed.
                  // in fact it could be Canceled with nothing happening.
                  // All we are going to do is hope for the best
                  // The documented norm for Optimized Moves is pdwEffect=None, so leave it
                ShDragDrop?.Invoke(m_DropList, m_LastNode, grfKeyState, pdwEffect);
            }
            ResetPrevTarget();
            int cnt = Marshal.Release(m_DragDataObj); // get rid of cnt added in DragEnter
            m_DragDataObj = IntPtr.Zero;
            return 0;
        }
    }
}
