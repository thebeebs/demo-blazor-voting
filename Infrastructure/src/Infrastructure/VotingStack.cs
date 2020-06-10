using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Lambda.EventSources;
using Amazon.CDK.AWS.APIGatewayv2;
using Amazon.CDK.AWS.DynamoDB;
using CfnDeploymentProps = Amazon.CDK.AWS.APIGatewayv2.CfnDeploymentProps;
using CfnStageProps = Amazon.CDK.AWS.APIGatewayv2.CfnStageProps;

namespace Infrastructure
{
    public class VotingStack : Stack
    {
        private string CREDENTAILS_ARN;
        private string LAMBDA_URI;

        internal VotingStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {

            var table = new Table(this, "Connections", new TableProps
            {
                PartitionKey = new Attribute
                {
                    Name = "connectionId",
                    Type = AttributeType.STRING
                },
                ServerSideEncryption = true,
                TableName = "Connections"
            });
            
            
            var onconnectFunction = new Function(this, "onconnectfunction", new FunctionProps
            {
                Tracing = Tracing.ACTIVE,
                Runtime = Runtime.DOTNET_CORE_2_1,
                Timeout = Duration.Seconds(30),
                MemorySize = 256,
                Environment = new Dictionary<string, string>()
                {
                    { "TABLE_NAME", table.TableName }
                },
                FunctionName = "OnConnect",
                Code = Code.FromAsset("./assets/OnConnect.zip"),
                Handler = "OnConnect::OnConnect.Function::FunctionHandler"
            });
            
            var onDisconnectFunction = new Function(this, "ondisconnectfunction", new FunctionProps
            {
                Tracing = Tracing.ACTIVE,
                Runtime = Runtime.DOTNET_CORE_2_1,
                Timeout = Duration.Seconds(30),
                MemorySize = 256,
                Environment = new Dictionary<string, string>()
                {
                    { "TABLE_NAME", table.TableName }
                },
                FunctionName = "OnDisconnect",
                Code = Code.FromAsset("./assets/OnDisconnect.zip"),
                Handler = "OnDisconnect::OnDisconnect.Function::FunctionHandler"
            });
            
            var sendMessageFunction = new Function(this, "sendmessagefunction", new FunctionProps
            {
                Tracing = Tracing.ACTIVE,
                Runtime = Runtime.DOTNET_CORE_2_1,
                Timeout = Duration.Seconds(30),
                MemorySize = 256,
                Environment = new Dictionary<string, string>()
                {
                    { "TABLE_NAME", table.TableName }
                },
                FunctionName = "SendMessage",
                Code = Code.FromAsset("./assets/SendMessage.zip"),
                Handler = "SendMessage::SendMessage.Function::FunctionHandler"
            });


            table.GrantReadWriteData(onconnectFunction);
            table.GrantReadWriteData(onDisconnectFunction);
            table.GrantReadWriteData(sendMessageFunction);

//            var api = new Amazon.CDK.AWS.APIGatewayv2.CfnApi(this, "api", new CfnApiProps
//            {
//                Name = "apicdk",
//                ProtocolType = "WEBSOCKET",
//                RouteSelectionExpression = "$request.body.message",
//            });

//            LAMBDA_URI =
//                $"arn:aws:apigateway:{region}:lambda:path/2015-03-31/functions/{onconnectFunction.FunctionArn}/invocations";
//
//            
//            var integration =
//                new Amazon.CDK.AWS.APIGatewayv2.CfnIntegration(this, "integration", new CfnIntegrationProps
//                    {
//                        ApiId = api.Ref,
//                        IntegrationType = "AWS_PROXY",
//                        CredentialsArn = CREDENTAILS_ARN,
//                        IntegrationUri = LAMBDA_URI
//                    }
//                );
//            
//            var route = new Amazon.CDK.AWS.APIGatewayv2.CfnRoute(this, "route", new CfnRouteProps
//            {
//                RouteKey = "RouteKey1",
//                AuthorizationType = "NONE",
//                ApiId = api.Ref,
//                Target ="integrations/" + integration.Ref,
//            });
//            
//
//            var deployment = new Amazon.CDK.AWS.APIGatewayv2.CfnDeployment(this, "deployment", new CfnDeploymentProps
//            {
//                ApiId = api.Ref,
//            });
//            
//
//            var stage = new Amazon.CDK.AWS.APIGatewayv2.CfnStage(this, "stage", new CfnStageProps {
//                ApiId = api.Ref,
//                StageName = "Prod",
//                DeploymentId = deployment.Ref
//            });
             
//            new CfnOutput(this, "APIGateWayURL", new CfnOutputProps
//            {
//                Value = $"wss://{stage.ApiId}.execute-api.{region}.amazonaws.com/{stage.StageName}"
//            });

            /* 
                        var startJobFunction = new Function(this, "StartJobFunction", new FunctionProps
                        {
                            Tracing = Tracing.ACTIVE,
                            Runtime = Runtime.DOTNET_CORE_2_1,
                            Timeout = Duration.Seconds(30),
                            MemorySize = 256,
                            Environment = new Dictionary<string, string>()
                            {
                                { "TRANSCRIBE_OUTPUT_BUCKET", outputBucket.BucketName },
                                { "TRANSCRIBE_LANGUAGE_CODE", languageCode }
                            },
                            FunctionName = "StartTranscriptionJob",
                            Code = Code.FromAsset("./assets/StartTranscriptionJob.zip"),
                            Handler = "StartTranscriptionJob::StartTranscriptionJob.Function::FunctionHandler",
                            Events = new[] 
                            {
                                new Api(inputBucket, new S3EventSourceProps
                                {
                                    Events = new [] { EventType.OBJECT_CREATED }
                                })
                            }
                        });

                        var notifyCompletionFunction = new Function(this, "NotifyCompleteFunction", new FunctionProps
                        {
                            Tracing = Tracing.ACTIVE,
                            Runtime = Runtime.DOTNET_CORE_2_1,
                            Timeout = Duration.Seconds(30),
                            MemorySize = 256,
                            Environment = new Dictionary<string, string>()
                            {
                                { "TRANSCRIBE_TOPIC_ARN", notificationTopic.TopicArn }
                            },
                            FunctionName = "NotifyTranscriptionJobComplete",
                            Code = Code.FromAsset("./assets/NotifyTranscriptionJobComplete.zip"),
                            Handler = "NotifyTranscriptionJobComplete::NotifyTranscriptionJobComplete.Function::FunctionHandler",
                            Events = new[]
                            {
                                new S3EventSource(outputBucket, new S3EventSourceProps
                                {
                                    Events = new [] { EventType.OBJECT_CREATED }
                                })
                            }
                        });

                        new CfnOutput(this, "APIGateWayURL", new CfnOutputProps
                        {
                            Value = inputBucket.BucketName
                        }); */
        }
    }
}
