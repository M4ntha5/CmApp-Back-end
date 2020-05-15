using System.Threading.Tasks;
using Xunit;

namespace LambdaFunction.Tests
{
    public class FunctionTest
    {
        [Fact]
        public async Task Handler_can_return_response()
        {
            // Init function
            var function = new Function();
            // Get response from function
            await function.Handler();
        }
    }
}