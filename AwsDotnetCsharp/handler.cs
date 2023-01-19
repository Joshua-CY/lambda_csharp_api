using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AwsDotnetCsharp
{
    public class Handler
    {
        public APIGatewayProxyResponse Hello(APIGatewayProxyRequest request, ILambdaContext context)
        {
            // Log entries show up in CloudWatch
            context.Logger.LogLine("Example log entry\n");

            var body = JObject.Parse(request.Body);
            var account = body["account"];
            // 取得url中最後一個名稱
            var urlEndPoint = request.Resource;

            // call 其他API
            var url = "https://193juwel0k.execute-api.ap-northeast-1.amazonaws.com/lab";
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            var data = "{\"name\": \"Joshua\", \"email\": \"test.com.tw\"}";
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            var httpResult = "";
     
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                httpResult = streamReader.ReadToEnd();
                Console.WriteLine(httpResult);
            }
            Console.WriteLine(httpResponse.StatusCode);
            

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                // Body = "{ \"Call CRM OK，登入者\" : \"" + account + "\" }",
                Body = "{ \"URL\" : \"" + urlEndPoint + "\" }" + "{ \"Call CRM OK\" : \"" + httpResult + "\" }",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };

            return response;
        }
    }


    public class Response
    {
        public string Message { get; set; }
        public Request Request { get; set; }

        public Response(string message, Request request)
        {
            Message = message;
            Request = request;
        }
    }

    public class Request
    {
        internal string Body;

        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public string Key3 { get; set; }

        public Request(string key1, string key2, string key3)
        {
            Key1 = key1;
            Key2 = key2;
            Key3 = key3;
        }
    }


}