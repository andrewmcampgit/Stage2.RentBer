using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RentBer.Data_Access;
using RentBer.Models;

namespace RentBer.Controllers
{
    public class RentalPaymentsController : ApiController
    {
        // GET: api/RentalPayments
        public IHttpActionResult Get(Guid? rentalAgreementId = null, Guid? ownerId = null, DateTime? dueAfterDate = null)
        {
            var rentalPaymentDao = new RentalPaymentDao();
            if (rentalAgreementId.HasValue || ownerId.HasValue || dueAfterDate.HasValue)
            {
                return Ok(rentalPaymentDao.GetFilteredRentalPayments(rentalAgreementId, ownerId, dueAfterDate)
                .OrderByDescending(rp => rp.DueDate));
            }
            else
            {
                return Ok(rentalPaymentDao.GetAllRentalPayments());
            }
        }

        // GET: api/RentalPayments/5
        public IHttpActionResult Get(Guid id)
        {
            var rentalPaymentDao = new RentalPaymentDao();
            var foundRentalPayment = rentalPaymentDao.GetRentalPaymentById(id);
            if (foundRentalPayment == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(foundRentalPayment);
            }
        }

        // POST: api/RentalPayments
        public IHttpActionResult Post([FromBody] EditableRentalPayment value)
        {
            var rentalPaymentDao = new RentalPaymentDao();
            var newRentalPayment = rentalPaymentDao.AddNewRentalPayment(value);
            return Ok(newRentalPayment);
        }

        // PUT: api/RentalPayments/5
        public IHttpActionResult Put(Guid id, [FromBody] EditableRentalPayment value)
        {
            var rentalPaymentDao = new RentalPaymentDao();
            var updatedRentalPayment = rentalPaymentDao.UpdateRentalPayment(id, value);
            if (updatedRentalPayment == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(updatedRentalPayment);
            }
        }

        // DELETE: api/RentalPayments/5
        public IHttpActionResult Delete(Guid id)
        {
            var rentalPaymentDao = new RentalPaymentDao();
            var didDelete = rentalPaymentDao.DeleteRentalPayment(id);
            if (didDelete)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return NotFound();
            }
        }
    }
}