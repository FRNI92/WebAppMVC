
namespace Database.ReposResult;


/// <summary>
/// ReposResult<T> can take anything with itself. list, bool, int
/// you need to have Succeded and statuscode
/// error is optional
/// Result is optional
/// So I cant sen the whole dto back. but I have to include the bool och a code. error is like errormessage and is optional but could be usefull when debugging
/// </summary>
/// <typeparam name="T"></typeparam>
public class ReposResult<T>
{
    public bool Succeeded { get; set; }
    public int StatusCode { get; set; }
    public string? Error { get; set; }
    public T? Result { get; set; }
}
