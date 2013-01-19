namespace TinaORM.Core
{
    public interface ISerializer
    {
        string Serialize(object o);

        T Deserialize<T>(string s);
    }
}