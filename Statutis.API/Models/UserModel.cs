namespace Statutis.API.Models;

public class UserModel<T>
{
    public UserModel(T data, bool res)
    {
        Data = data;
        Res = res;
    }

    public T Data { get; set; }
    public bool Res { get; set; }
}