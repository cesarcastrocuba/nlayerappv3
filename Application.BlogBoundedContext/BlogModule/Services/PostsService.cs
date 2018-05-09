namespace NLayerApp.Application.BlogBoundedContext.BlogModule.Services
{
    using NLayerApp.Application.BlogBoundedContext.DTO;
    using NLayerApp.Application.Seedwork;
    using NLayerApp.Application.Seedwork.Common;
    using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.PostAgg;
    using NLayerApp.Domain.Seedwork.Specification;
    using NLayerApp.Infrastructure.Crosscutting.Localization;
    using NLayerApp.Infrastructure.Crosscutting.Validator;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PostsService : Service<Post,PostDTO>, IPostsService
    {
        IPostRepository _postRepository;

        protected ILocalization _resources;
        protected IEntityValidator _validator;
        public PostsService(IPostRepository postRepository)
            :base(postRepository)
        {
            _postRepository = postRepository;

            _resources = _resources;
            _validator = EntityValidatorFactory.CreateValidator();
        }
        public override void Dispose()
        {
            _postRepository.Dispose();

            base.Dispose();
        }

        public async Task<List<PostDTO>> GetAllDTOAsync(string q)
        {
            ISpecification<Post> specification = PostSpecifications.PostsContainsTitleOrContent(q);

            var posts = await _postRepository.AllMatchingAsync(specification);

            if (posts != null && posts.Any())
            {
                return posts.ProjectedAsCollection<PostDTO>();
            }

            return null;

        }

        public async override Task<PostDTO> GetDTOAsync(object id)
        {
            var item = await _postRepository.GetAsync(id);
            if (item == null) return null;
            return item.ProjectedAs<PostDTO>();
        }

        public async override Task<PostDTO> AddAsync(PostDTO postDTO)
        {
            if (postDTO == null)
            {
                throw new ArgumentException(_resources.GetStringResource(LocalizationKeys.Application.validation_No_Records_Found_Error));
            }

            Post post = PostFactory.CreatePost(postDTO.Title, postDTO.Content, postDTO.BlogId);

            if (_validator.IsValid<Post>(post))
            {
                _postRepository.Add(post);
                
                await _postRepository.UnitOfWork.CommitAsync();
                return post.ProjectedAs<PostDTO>();
            }
            else
            {
                throw new ApplicationValidationErrorsException(_validator.GetInvalidMessages<Post>(post));
            }
        }



        public async override Task<PostDTO> ModifyAsync(object id, PostDTO postDTO)
        {
            if (postDTO == null)
            {
                throw new ArgumentException(_resources.GetStringResource(LocalizationKeys.Application.validation_No_Records_Found_Error));
            }

            if ((int)id != postDTO.PostId)
            {
                throw new ArgumentException(_resources.GetStringResource(LocalizationKeys.Application.validation_BadRequest));
            }

            Post post = await _postRepository.GetAsync(id);

            if (post == null) return null;

            post.Title = post.Title;
            post.Content = post.Content;
            post.BlogId = post.BlogId;
            
            if (_validator.IsValid<Post>(post))
            {
                _postRepository.Modify(post);
                await _postRepository.UnitOfWork.CommitAsync();

                return post.ProjectedAs<PostDTO>();
            }
            else
            {
                throw new ApplicationValidationErrorsException(_validator.GetInvalidMessages<Post>(post));
            }
        }

        public override async Task RemoveAsync(object id)
        {
            var item = await _postRepository.GetAsync(id);
            if (item == null) return;

            _postRepository.Remove(item);

            await _postRepository.UnitOfWork.CommitAsync();
        }

        
    }
}
