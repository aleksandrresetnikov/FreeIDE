namespace FreeIDE.Common.API
{
    public interface IPathItemFileFunctions
    {
        void Delete();
        void MoveTo(string pathTo);
        void CreateFile();
        void CreateDirectory();
    }
}
