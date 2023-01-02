 

namespace web.api.Utility.Swagger {
    public class RemoveVersionParameterFilter : IOperationFilter {
        public void Apply(OpenApiOperation operation, OperationFilterContext context) {
            if (operation.Parameters.Count > 0) {
                var versionParameter = operation.Parameters.Single(p => p.Name == "version");
                operation.Parameters.Remove(versionParameter);
            }
        }
    }
}

