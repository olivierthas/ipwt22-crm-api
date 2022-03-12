using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Crm.Link.Api.Extensions
{
    public static class ModelStateExtention
    {
        public static IEnumerable<string> ErrorsToString(this ModelStateDictionary modelState)
        {
            List<string> messages = new();
            modelState.Values
                .Select(e => e.Errors)
                .Select(e => e.Select(m => m.ErrorMessage))
                .Foreach(e => messages.AddRange(e));

            return messages;
        }
    }
}
