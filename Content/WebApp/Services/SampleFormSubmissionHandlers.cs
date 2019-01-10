using cloudscribe.Forms.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

// you can implement named forms sumbission handlers as shown below to process form submissions with custom logic.
// 

namespace WebApp.Services
{
    public class FakeFormSubmissionHandler1 : IHandleFormSubmission
    {
        public FakeFormSubmissionHandler1(ILogger<FakeFormSubmissionHandler1> logger)
        {
            _log = logger;
        }

        private readonly ILogger _log;


        public string Name { get; } = "FakeHandler 1";
        public string CatalogId { get; } = "default";
        public bool IsGlobal { get; } = false;

        public Task Handle(FormResponse response)
        {
            _log.LogInformation("FakeHandler 1 invoked");

            //var responseHelper = new FormResponseJsonHelper(response.ResponseJson);

            return Task.CompletedTask;
        }
    }

    public class FakeFormSubmissionHandler2 : IHandleFormSubmission
    {
        public FakeFormSubmissionHandler2(ILogger<FakeFormSubmissionHandler2> logger)
        {
            _log = logger;
        }

        private readonly ILogger _log;

        public string Name { get; } = "FakeHandler 2";
        public string CatalogId { get; } = "default";
        public bool IsGlobal { get; } = false;

        public Task Handle(FormResponse response)
        {
            _log.LogInformation("FakeHandler 1 invoked");

            //var responseHelper = new FormResponseJsonHelper(response.ResponseJson);

            return Task.CompletedTask;
        }
    }

}
