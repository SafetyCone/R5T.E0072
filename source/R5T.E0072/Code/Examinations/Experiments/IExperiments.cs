using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 

using Polenter.Serialization;

using R5T.T0141;
using R5T.T0162;
using R5T.T0180.Extensions;
using R5T.T0212.F000;


namespace R5T.E0072
{
    [ExperimentsMarker]
    public partial interface IExperiments : IExperimentsMarker
    {
        public void DeserializeStrangeObject()
        {
            /// Inputs.
            var dataFilePath = Instances.FilePaths.OutputDataFilePath.ToFilePath();


            /// Run.
            var serializer = new SharpSerializer(true);

            // Does not work!
            var memberDocumentationsByIdentityName = (KeyValuePair<IIdentityName, MemberDocumentation>[])serializer.Deserialize(dataFilePath.Value);
                
            Console.WriteLine($"{memberDocumentationsByIdentityName.Length}: count");
        }

        public async Task GetAndSerializeStrangeObject()
        {
            /// Inputs.
            var dotnetPackName = Instances.DotnetPackNames.Microsoft_NETCore_App_Ref;
            var targetFramework = Instances.TargetFrameworkMonikers.NET_6;
            var dataFilePath = Instances.FilePaths.OutputDataFilePath.ToFilePath();


            /// Run.
            var documentationXmlFilePaths = Instances.DotnetPackPathOperator.GetDocumentationXmlFilePaths(
                dotnetPackName,
                targetFramework);

            var documentationTarget = new DotnetFrameworkTarget()
            {
                TargetFrameworkMoniker = targetFramework,
            };

            var memberDocumentationsByIdentityName = await Instances.DocumentationFileOperator.Get_MemberDocumentationsByIdentityName(
                documentationXmlFilePaths,
                documentationTarget);

            // Binary serialize.
            var serializer = new SharpSerializer(true);

            // Fails, since one of the types does not implement equality.
            // Fails on deserialization, since somehow SharpSerializer can't handle a dictionary?
            //var serializationObject = memberDocumentationsByIdentityName;

            // Try as an array of key-value pairs.
            // Fails, says some object is null?
            var serializationObject = memberDocumentationsByIdentityName.ToArray();

            serializer.Serialize(
                serializationObject,
                dataFilePath.Value);
        }
    }
}
