using Template.API.Infrastructure;
using Template.Configurations.ConfigurationReader;

Builder.ConfigurationReader ??= ConfigurationSourceExtension.TemplateConfigurationReader();

var builder = Builder.GetBuilder(args);

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

app.UseMiddleware<LanguageMiddleware>();

app.UseCors("AllowSpecificOrigin");

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});

app.MapControllers();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.Run();