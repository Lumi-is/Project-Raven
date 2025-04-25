
public interface IDataHandler 
{
    void CreateDirectory(string filePath);
    void SaveFile<T>(T dataToStore, string filePath);
    void DeleteFile(string filePath);
}
