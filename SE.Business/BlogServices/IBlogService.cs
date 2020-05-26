using SE.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Business.BlogServices
{
    public interface IBlogService
    {
        int InsertBlog(BlogInsertDto blogInsertDto);
        int UpdateBlog(BlogUpdateDto blogUpdateDto);
        void DeleteBlog(int blogId, string userId);
        List<BlogListDto> GetAllBlogListByUserId(string userId);
        BlogDetailDto GetBlogDetailBySeoUrl(string seoUrl);
        BlogUpdateDto GetBlogUpdateBySeoUrl(string seoUrl, string userId);
        BlogDetailDto GetBlogDetailById(int Id);


    }
}
