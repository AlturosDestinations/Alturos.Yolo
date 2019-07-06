using Alturos.Yolo.Model;
using Alturos.Yolo.WebService.Contract;
using System;
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
