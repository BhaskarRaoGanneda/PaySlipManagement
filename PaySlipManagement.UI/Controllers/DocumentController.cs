using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PaySlipManagement.Common.Models;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;

namespace PaySlipManagement.UI.Controllers
{
    public class DocumentController : Controller
    {

        private readonly APIServices _apiServices;
        private readonly ApiSettings _apiSettings;
        public DocumentController(APIServices apiServices, IOptions<ApiSettings> apiSettings)
        {
            _apiServices = apiServices;
            _apiSettings = apiSettings.Value;
        }
        public async Task<IActionResult> Index()
        {
            //var documentTypes = new List<string>
            //{
            //   "10th Certificate",
            //   "Intermediate/Diploma Certificate",
            //   "Graduation Certificate",
            //   "Post Graduation Certificate",
            //   "Aadhar Card",
            //   "PAN Card",
            //   "Passport Size Photo",
            //   "Relieving Letter",
            //   "PaySlips",
            //   "Offer Letter",
            //   "Resignation Email",
            //   "Form-16",
            //   "Resume",
            //   "Latest UAN Membership Certificate"
            //};
            //var viewModel = new DocumentViewModel
            //{
            //    DocumentType = documentTypes
            //};

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DownloadDocument(string empCode, string documentType)
        {
            if (string.IsNullOrEmpty(empCode))
            {
                empCode = TempData["Emp_Code"] as string;
                TempData.Keep("Emp_Code"); // Keep TempData for further requests
            }
            documentType = documentType?.Trim().ToLower(); // Normalize document type
                                                           // Fetch document details from the API
            var url = $"{_apiSettings.DocumentEndpoint}/GetDocumentsByIdAsync/{empCode}/{documentType}";
            var doc = await _apiServices.GetAsync<DocumentViewModel>(url);

            if (doc == null || doc.FileData == null)
            {
                return NotFound("Document not found.");
            }

            return File(doc.FileData, "application/octet-stream", doc.FileName);
        }

        [HttpPost]
        public async Task<IActionResult> ViewDocument(string empCode, string documentType)
        {
            if (string.IsNullOrEmpty(empCode))
            {
                empCode = TempData["Emp_Code"] as string;
                TempData.Keep("Emp_Code"); // Keep TempData for further requests
            }
            documentType = documentType?.Trim().ToLower(); // Normalize document type
                                                           // Fetch document details from the API
            var url = $"{_apiSettings.DocumentEndpoint}/GetDocumentsByIdAsync/{empCode}/{documentType}";
            var doc = await _apiServices.GetAsync<DocumentViewModel>(url);

            if (doc == null || doc.FileData == null)
            {
                return NotFound("Document not found.");
            }

            // Determine the MIME type based on the document type
            string mimeType;
            switch (Path.GetExtension(doc.FileName)?.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    mimeType = "image/jpeg";
                    break;
                case ".png":
                    mimeType = "image/png";
                    break;
                default:
                    mimeType = "application/pdf";
                    break;
            }

            return File(doc.FileData, mimeType);
        }

        [HttpGet]
        //[Route("Document/create")] // Specify a unique route for the GET method
        public async Task<IActionResult> Create()
        {
            
            return View();
        }

        [HttpPost]
        //[Route("Document/create")] // Specify the same route for the POST method
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<IFormFile> files, List<string> documentTypes, string? empCode)
        {
            empCode = TempData["Emp_Code"] as string; // Fetch the Emp_Code
            if (ModelState.IsValid)
            {
                // Check if any files were uploaded
                if (files == null || files.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, "Please upload at least one valid file.");
                    return View("Create");
                }

                // Check if the number of files matches the number of document types
                if (files.Count != documentTypes.Count)
                {
                    ModelState.AddModelError(string.Empty, "The number of files must match the number of document types.");
                    return View("Create");
                }

                // Allowed MIME types
                var allowedFileTypes = new[]
                {
                   "application/pdf",
                   "image/jpeg",
                   "image/png"
                };
                //ViewBag.Emp_Code = empCode; // Pass to the view
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    var documentType = documentTypes[i]; // Get the corresponding document type

                    // Validate the file input
                    if (file == null || file.Length == 0)
                    {
                        ModelState.AddModelError(string.Empty, "Please upload a valid file.");
                        return View("Create");
                    }

                    // Validate the file format (allow PDFs and images)
                    if (!allowedFileTypes.Contains(file.ContentType.ToLower()))
                    {
                        ModelState.AddModelError(string.Empty, "Only PDF, JPEG, and PNG files are allowed.");
                        return View("Create");
                    }

                    // Create the document model
                    var document = new Document
                    {
                        Id = 0, // Assuming 0 for new records
                        FileName = file.FileName,
                        DocumentType = documentType, // Use the document type from the hidden input
                        Emp_Code = empCode
                    };

                    // Read the file data
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        document.FileData = stream.ToArray();
                    }

                    // Call the API to save the document
                    var response = await _apiServices.PostAsync($"{_apiSettings.DocumentEndpoint}/CreateDocumentAsync", document);

                    // Handle the response from the API
                    if (string.IsNullOrEmpty(response) || !(response == "Document Uploaded Successfully" || response == "true"))
                    {
                        // Add appropriate error messages
                        if (!string.IsNullOrEmpty(response))
                        {
                            ModelState.AddModelError(string.Empty, response);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Failed to upload document. Please try again.");
                        }
                        return View("Create");
                    }
                }

                TempData["SuccessMessage"] = "Documents uploaded successfully.";
                return RedirectToAction("Index", "Employee"); // Redirect to Employee Index View
            }

            // If we reach here, something went wrong
            ModelState.AddModelError(string.Empty, "Invalid upload attempt.");
            return View("Create");
        }
    }
}
