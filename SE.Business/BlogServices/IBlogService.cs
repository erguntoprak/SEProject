using SE.Core.DTO;
using SE.Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE.Business.BlogServices
{
    public interface IBlogService
    {
        Task<IDataResult<int>> InsertBlogAsync(BlogInsertDto blogInsertDto);
        Task<IDataResult<int>> UpdateBlogAsync(BlogUpdateDto blogUpdateDto);
        Task<IResult> DeleteBlogAsync(int blogId, string userId);
        Task<IDataResult<IEnumerable<BlogListDto>>> GetAllBlogListByUserIdAsync(string userId);
        Task<IDataResult<IEnumerable<BlogListDto>>> GetAllBlogListByUserNameAsync(string userName);
        Task<IDataResult<IEnumerable<BlogListDto>>> GetAllBlogListAsync();
        Task<IDataResult<IEnumerable<BlogListDto>>> GetAllBlogViewListAsync();
        Task<IDataResult<BlogDetailDto>> GetBlogDetailBySeoUrlAsync(string seoUrl);
        Task<IDataResult<BlogUpdateDto>> GetBlogUpdateBySeoUrlAsync(string seoUrl, string userId);
        Task<IDataResult<BlogDetailDto>> GetBlogDetailByIdAsync(int Id);
        Task<IResult> UpdateBlogActivateAsync(int blogId, bool isActive);



    }
}
