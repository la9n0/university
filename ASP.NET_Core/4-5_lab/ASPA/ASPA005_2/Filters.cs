using DAL004;

namespace ASPA005_2
{
    public class SurnameFilter : IEndpointFilter
    {
        private IRepository _repository;
        public SurnameFilter(IRepository repository)
        {
            _repository = repository;
        }
        public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            Celebrity? celebrity = context.GetArgument<Celebrity>(0);
            if (celebrity == null) throw new ValueErrorException("POST /Celebrities error, celebrity is null");
            if (celebrity.Surname == null || celebrity.Surname.Length < 2)
                throw new ValueErrorException("POST /Celebrities error, Surname is wrong");
            Celebrity[] celebrities = _repository.getAllCelebrities();
            if (celebrities.Any(n => n.Surname == celebrity.Surname))
                throw new ValueErrorException("POST /Celebrities error, Surname is doubled");
            return next(context);
        }
    }

    public class PhotoExistFilter : IEndpointFilter
    {
        private IRepository _repository;
        public PhotoExistFilter(IRepository repository)
        {
            _repository = repository;
        }
        public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            Celebrity? celebrity = context.GetArgument<Celebrity>(0);
            var result = next(context);
            if (!File.Exists(celebrity.PhotoPath))
                context.HttpContext.Response.Headers["X-Celebrity"] = $"NotFound={celebrity.PhotoPath}";
            return result;
        }
    }

    public class PutFilter : IEndpointFilter
    {
        private IRepository _repository;
        public PutFilter(IRepository repository)
        {
            _repository = repository;
        }
        public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            Celebrity? celebrity = context.GetArgument<Celebrity>(1);
            if (celebrity == null) throw new ValueErrorException("PUT /Celebrities error, celebrity is null");
            if (celebrity.Surname == null || celebrity.Surname.Length < 2)
                throw new ValueErrorException("PUT /Celebrities error, Surname is wrong");
            if (celebrity.Firstname == null || celebrity.Firstname.Length < 2)
                throw new ValueErrorException("PUT /Celebrities error, Firstname is wrong");
            return next(context);
        }
    }

    public class DeleteFilter : IEndpointFilter
    {
        private IRepository _repository;
        public DeleteFilter(IRepository repository)
        {
            _repository = repository;
        }
        public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            int id = context.GetArgument<int>(0);
            var celebrity = _repository.GetCelebrityById(id);
            if (celebrity == null) throw new DelCelebrityException($"Celebrity Id = {id} not found", "/Celebrities");
            return next(context);
        }
    }
}
