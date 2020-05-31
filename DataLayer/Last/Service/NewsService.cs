using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Last
{
    public class NewsService : INewsService
    {
        readonly Repository<NewsEntity> _repo;
        public NewsService(Repository<NewsEntity> repo)
        {
            _repo = repo;
        }
        public IEnumerable<NewsEntity> GetAllNews()
        {
            var getAll = @"SELECT * FROM News";
            return _repo.Read(getAll);
        }
        public bool AddNews(NewsViewModel viewModel)
        {
            try
            {
                var add = $@"
 INSERT INTO News(Title,Description,PublishDate)   VALUES(@Title,@Description,@PublishDate)    
";
                _repo.Open();
                _repo.Create(add, new NewsEntity()
                {
                    Description = viewModel.Description,
                    PublishDate = DateTime.Now.ToString(),
                    Title = viewModel.Title
                });
                _repo.Close();
                return true;
            }
            catch (Exception)
            {
                throw;
                return false;
            }
        }
    }
}
