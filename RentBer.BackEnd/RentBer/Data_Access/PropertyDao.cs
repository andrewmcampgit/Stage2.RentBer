using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RentBer.Models;

namespace RentBer.Data_Access
{
    public class PropertyDao : DaoBase
    {
        public Property[] GetAllProperties()
        {
            var propertyCol = db.GetCollection<Property>("Properties");
            return propertyCol.FindAll().ToArray();
        }

        public Property GetPropertyById(Guid propertyId)
        {
            var propertyCol = db.GetCollection<Property>("Properties");
            return propertyCol.FindById(propertyId);
        }

        public Property AddNewProperty(EditableProperty editableProperty)
        {
            if (!editableProperty.OwnerId.HasValue)
            {
                throw new ValidationException("OwnerId is required to add a property.");
            }
            var propertyCol = db.GetCollection<Property>("Properties");
            var newProperty = new Property()
            {
                Id = Guid.NewGuid(),
                OwnerId = editableProperty.OwnerId.Value,
                StreetAddress = editableProperty.StreetAddress,
                City = editableProperty.City,
                State = editableProperty.State,
                Zip = editableProperty.Zip
            };

            propertyCol.Insert(newProperty);
            return newProperty;
        }

        public Property[] GetPropertiesForOwner(Guid ownerId)
        {
            var propertyCol = db.GetCollection<Property>("Properties");
            return propertyCol.Find(p => p.OwnerId == ownerId).ToArray();
        }

        public Property UpdateProperty(Guid propertyId, EditableProperty editableProperty)
        {
            var propertyCol = db.GetCollection<Property>("Properties");
            var foundProperty = propertyCol.FindById(propertyId);

            if (foundProperty == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(editableProperty.StreetAddress))
            {
                foundProperty.StreetAddress = editableProperty.StreetAddress;
            }

            if (!string.IsNullOrEmpty(editableProperty.City))
            {
                foundProperty.City = editableProperty.City;
            }

            if (!string.IsNullOrEmpty(editableProperty.State))
            {
                foundProperty.State = editableProperty.State;
            }

            if (!string.IsNullOrEmpty(editableProperty.Zip))
            {
                foundProperty.Zip = editableProperty.Zip;
            }

            propertyCol.Update(foundProperty);
            return foundProperty;
        }

        public bool DeleteProperty(Guid propertyId)
        {
            var propertyCol = db.GetCollection<Property>("Properties");
            return propertyCol.Delete(propertyId);
        }
    }
}