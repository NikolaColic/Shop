using AutoMapper;
using Data.Entities;
using Enigmatry.Grpc;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Shop.Service.Interfaces;

namespace Shop.Service.Implementations.Grpc
{
    public class ArticleGrpcServices : IGrpcArticleService
    {
        private readonly ArticleGrpc.ArticleGrpcClient _vendorService;
        private readonly IMapper _mapper;

        public ArticleGrpcServices(IMapper mapper, IConfiguration configuration)
        {
            _vendorService = new ArticleGrpc.ArticleGrpcClient(GrpcChannel.ForAddress(configuration["Grpc:VendorUrl"]));
            _mapper = mapper;
        }

        public async Task<List<Article>> Buy(List<int> keys)
        {
            var articlesGrpc = await _vendorService.BuyAsync(new ArticleBuyGrpc() { Articles = string.Join(',', keys) });
            var articles = _mapper.Map<List<Article>>(articlesGrpc.Articles);
            return articles;
        }

        public async Task<List<Article>> GetAll()
        {
            var articlesGrpc = await _vendorService.GetAllAsync(new ArticleEmptyGrpc());
            var articles = _mapper.Map<List<Article>>(articlesGrpc.Articles);
            return articles;
        }

        public async Task<Article> GetById(int id)
        {
            var articleGrpc = await _vendorService.GetByIdAsync(new ArticleIdGrpc() { Key = id });
            var article = _mapper.Map<Article>(articleGrpc);
            return article;
        }
    }
}
