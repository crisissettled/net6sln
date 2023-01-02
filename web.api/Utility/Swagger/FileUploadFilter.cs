namespace web.api.Utility.Swagger {
    //https://www.bilibili.com/video/BV1uD4y1b7eW/?p=7&spm_id_from=pageDriver&vd_source=4b77e51739cdf477312534403b78daa9

    /// <summary>
    /// Refer to 
    /// </summary>
    public class FileUploadFilter : IOperationFilter {
        public void Apply(OpenApiOperation operation, OperationFilterContext context) {
            const string FileUploadContentType = "multipart/form-data";
            if (operation.RequestBody == null ||
                !operation.RequestBody.Content.Any(x =>
                        x.Key.Equals(FileUploadContentType, StringComparison.InvariantCultureIgnoreCase
                ))) {

                return;
            }

            if (context.ApiDescription.ParameterDescriptions[0].Type == typeof(IFormCollection)) {
                operation.RequestBody = new OpenApiRequestBody {
                    Description = "File Upload",
                    Content = new Dictionary<string, OpenApiMediaType> {
                        {
                            FileUploadContentType,
                            new OpenApiMediaType {
                                Schema = new OpenApiSchema {
                                    Type= "object",
                                    Required = new HashSet<string> {"file"},
                                    Properties = new Dictionary<string, OpenApiSchema> {
                                        { 
                                            "file",new OpenApiSchema() {
                                                Type= "string",
                                                Format="binary"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }
    }
}
