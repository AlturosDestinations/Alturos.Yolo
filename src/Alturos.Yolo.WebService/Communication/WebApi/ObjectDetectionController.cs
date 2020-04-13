using Alturos.Yolo.Model;
using Alturos.Yolo.WebService.Contract;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Alturos.Yolo.WebService.Communication.WebApi
{
    [RoutePrefix("ObjectDetection")]
    public class ObjectDetectionController : ApiController
    {
        private readonly IObjectDetection _objectDetection;

        public ObjectDetectionController(IObjectDetection objectDetection)
        {
            this._objectDetection = objectDetection;
        }

        /// <summary>
        ///  Detect object positions
        /// </summary>
        /// <param name="imageData">Image data</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Detect")]
        [ResponseType(typeof(YoloItem[]))]
        public IHttpActionResult Detect(byte[] imageData)
        {
            try
            {
                var items = this._objectDetection.Detect(imageData);
                return Ok(items);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        /// <summary>
        ///  Upload image as multi-part from data 
        ///  This example can be tested with pastman. http://localhost:8080//ObjectDetection//Upload 
        ///  Select Body->form data
        ///  Then select "file" from the key box before typing in (any) key name. Then select your image file in the value. 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Upload")]
        [ResponseType(typeof(YoloItem[]))]
        public async Task<IHttpActionResult> Detect()
        {
            // Get the HTTP request
            //HttpRequestMessage httpRequest = this.Request;
            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            // Get the first value from the form
            var file = provider.Contents[0];
                        
            // Read file as bytes
            var imageData = await file.ReadAsByteArrayAsync();
            try
            {
                // Pass byte array to wrapper
                var items = this._objectDetection.Detect(imageData);
                return Ok(items);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        /// <summary>
        ///  Detect object positions
        /// </summary>
        /// <param name="filePath">local file path</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DetectLocalPath")]
        [ResponseType(typeof(YoloItem[]))]
        public IHttpActionResult Detect(string filePath)
        {
            try
            {
                var items = this._objectDetection.Detect(filePath);
                return Ok(items);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }
    }
}
