using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using LiteDB;
using RentBer.Models;

namespace RentBer.Data_Access
{
    public class RentalPaymentDao : DaoBase
    {
        public RentalPayment[] GetAllRentalPayments()
        {
            var rentalPaymentCol = db.GetCollection<RentalPayment>("RentalPayments");
            return rentalPaymentCol.FindAll().ToArray();
        }

        public RentalPayment[] GetFilteredRentalPayments(Guid? rentalAgreementId = null, Guid? ownerId = null, DateTime? dueAfterDate = null)
        {
            var rentalPaymentCol = db.GetCollection<RentalPayment>("RentalPayments");
            var machingRentalPayments = rentalPaymentCol.FindAll();

            if (rentalAgreementId.HasValue)
            {
                machingRentalPayments =
                    machingRentalPayments.Where(rp => rp.RentalAgreementId == rentalAgreementId.Value);
            }

            if (ownerId.HasValue)
            {
                var rentalAgreementDao = new RentalAgreementDao();
                var rentalAgreements = rentalAgreementDao.GetRentalAgreementsByOwner(ownerId.Value)
                    .Select(ra => ra.Id);

                machingRentalPayments =
                    machingRentalPayments.Where(rp => rentalAgreements.Contains(rp.RentalAgreementId));
            }

            if (dueAfterDate.HasValue)
            {
                machingRentalPayments = machingRentalPayments.Where(rp => rp.DueDate > dueAfterDate.Value);
            }

            return machingRentalPayments.ToArray();
        }

        public RentalPayment GetRentalPaymentById(Guid id)
        {
            var rentalPaymentCol = db.GetCollection<RentalPayment>("RentalPayments");
            return rentalPaymentCol.FindById(id);
        }

        public RentalPayment AddNewRentalPayment(EditableRentalPayment editableRentalPayment)
        {
            if (!editableRentalPayment.RentalAgreementId.HasValue)
            {
                throw new ValidationException("Rental Agreement Id is required for new Rental Payments.");
            }

            var rentalPaymentCol = db.GetCollection<RentalPayment>("RentalPayments");
            var newRentalPayment = new RentalPayment()
            {
                Id = Guid.NewGuid(),
                RentalAgreementId = editableRentalPayment.RentalAgreementId.Value,
                DueDate = editableRentalPayment.DueDate ?? DateTime.MinValue,
                PaidDate = editableRentalPayment.PaidDate ?? null,
                IsPaid = editableRentalPayment.IsPaid ?? false
            };

            rentalPaymentCol.Insert(newRentalPayment);
            return newRentalPayment;
        }

        public RentalPayment UpdateRentalPayment(Guid rentalPaymentId, EditableRentalPayment editableRentalPayment)
        {
            var rentalPaymentCol = db.GetCollection<RentalPayment>("RentalPayments");
            var foundRentalPayment = rentalPaymentCol.FindById(rentalPaymentId);
            if (foundRentalPayment == null)
            {
                return null;
            }

            if (editableRentalPayment.RentalAgreementId.HasValue)
            {
                foundRentalPayment.RentalAgreementId = editableRentalPayment.RentalAgreementId.Value;
            }

            if (editableRentalPayment.DueDate.HasValue)
            {
                foundRentalPayment.DueDate = editableRentalPayment.DueDate.Value;
            }

            if (editableRentalPayment.IsPaid.HasValue)
            {
                foundRentalPayment.IsPaid = editableRentalPayment.IsPaid.Value;
            }
            if (editableRentalPayment.PaidDate.HasValue)
            {
                foundRentalPayment.PaidDate = editableRentalPayment.PaidDate.Value;
            }

            rentalPaymentCol.Update(foundRentalPayment);

            return foundRentalPayment;
        }

        public bool DeleteRentalPayment(Guid id)
        {
            var rentalPaymentCol = db.GetCollection<RentalPayment>("RentalPayments");
            return rentalPaymentCol.Delete(id);
        }
    }
}