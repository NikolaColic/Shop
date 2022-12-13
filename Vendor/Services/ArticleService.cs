using AutoMapper;
using Data.Entities;
using Enigmatry.Grpc;
using Grpc.Core;
using Infrastructure.Repository.Interfaces;
using Infrastructure.UnitOfWork.Interfaces;

namespace Vendor.Api.Services
{
    public class ArticleService : ArticleGrpc.ArticleGrpcBase
    {
        private readonly IUnitOfWork<Article> _uow;
        private readonly IMapper _mapper;
        private readonly IRepositoryList<Article> repositoryList;

        public ArticleService(IUnitOfWork<Article> uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
            repositoryList = uow?.Repository as IRepositoryList<Article>;
        }

        public override async Task<ArticleListGrpc> GetAll(ArticleEmptyGrpc request, ServerCallContext context)
        {
            var articles = await _uow.Repository.GetAll();
            var articlesGrpc = _mapper.Map<List<ArticleDtoGrpc>>(articles);
            return new ArticleListGrpc() { Articles = { articlesGrpc } };
        }

        public override async Task<ArticleDtoGrpc> GetById(ArticleIdGrpc request, ServerCallContext context)
        {
            var article = await _uow.Repository.GetById(request.Key);
            var articleDto = _mapper.Map<ArticleDtoGrpc>(article);
            return articleDto;
        }

        public async override Task<ArticleListGrpc> Buy(ArticleBuyGrpc request, ServerCallContext context)
        {
            var keys = request.Articles.Split(',').Select(e => int.Parse(e)).ToList();
            var articles = await repositoryList.GetByIds(keys);

            foreach (var article in articles)
            {
                article.IsSold = true;
                article.SoldDate = DateTime.Now;
                await _uow.Repository.Update(article);
            }

            await _uow.Commit();

            var articlesGrpc = _mapper.Map<List<ArticleDtoGrpc>>(articles);
            return new ArticleListGrpc() { Articles = { articlesGrpc } };
        }

    }
}
