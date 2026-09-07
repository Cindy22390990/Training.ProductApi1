namespace Training.ProductApi1.Models.DTOs;


public class PagedResult<T>
{
    public int TotalCount { get; set; }


    public int TotalPages { get; set; }


    public int PageIndex { get; set; }


    public int PageSize { get; set; }


    public List<T> Data { get; set; } = new();


}