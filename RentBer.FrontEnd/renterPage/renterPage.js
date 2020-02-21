const baseHostApi = "http://localhost:33819/api";

$(
    function () {
        var urlParams = new URLSearchParams(window.location.search);
        renterId = urlParams.get("renterId");

        $.ajax(`${baseHostApi}/renters/${renterId}`)
            .done(renderRenterPage)
            .fail(function (xhr, status, err) {
                alert("Ajax Failed. Is the backend running? Err:" + status)
            });;
    }
);

function renderRenterPage(renter) {
    $("#greeting-header").text("Hello " + renter.FirstName + " " + renter.LastName)
    $.ajax(`${baseHostApi}/rentalagreements?renterId=${renter.Id}`)
        .done(function (rentalAgreements) {
            let rentalAgreement = rentalAgreements[0];
            $.ajax(`${baseHostApi}/properties/${rentalAgreement.PropertyId}`)
                .done(renderPropertyDetails);

            $.ajax(`${baseHostApi}/owners/${rentalAgreement.OwnerId}`)
                .done(function (owner) {
                    renderRentalAgreementDetails(rentalAgreement, owner);
                    //added this below
                    $.ajax(`${baseHostApi}/rentalpayments?rentalAgreementId=${rentalAgreement.Id}`)
                        .done(function(rentalPayments){
                            var dueRentalPayments = []
                            var paidRentalPayments = []
                            for (let rentalPayment of rentalPayments){
                                if (rentalPayment.IsPaid == false){
                                    dueRentalPayments.push(rentalPayment);
                                }
                                else
                                {
                                    paidRentalPayments.push(rentalPayment);

                                }
                            }
                            renderDueRentalPayment(dueRentalPayments, rentalAgreement, owner);
                            renderPaidRentalPayment(paidRentalPayments, rentalAgreement, owner);

                        })
                })
        });
}

function renderPropertyDetails(property) {
    let propertyLineOneElem = $("<div>")
    propertyLineOneElem.text(`${property.StreetAddress},`);
    let propertyLineTwoElem = $("<div>");
    propertyLineTwoElem.text(`${property.City} ${property.State}`);
    let propertyLineThreeElem = $("<div>")
    propertyLineThreeElem.text(`${property.Zip}`);

    $("#property-details-div").append(propertyLineOneElem, propertyLineTwoElem, propertyLineThreeElem);
}

function renderRentalAgreementDetails(rentalAgreement, owner) {
    let rentalAgreementLineOne = $("<div>")
    rentalAgreementLineOne.text("Property Owner: " + owner.FirstName + " " + owner.LastName);
    let rentalAgreementLineTwo = $("<div>")
    rentalAgreementLineTwo.text("Monthly Rate: $" + rentalAgreement.MonthlyRate);

    $("#rental-agreement-div").append(rentalAgreementLineOne, rentalAgreementLineTwo);
}

// added this function
function renderDueRentalPayment(rentalPayments, rentalAgreement, owner){
    
    var rentalPaymentsDiv = $("#due-rental-payments-div");
    console.log(rentalPaymentsDiv);
    for (let rentalPayment of rentalPayments){
        //console.log(rentalPayments);
        var rentalPaymentElem = $("<div>");
        rentalPaymentElem.text(`$${rentalAgreement.MonthlyRate} due on ` +
        `${new Date(rentalPayment.DueDate).toLocaleDateString()} for ${owner.FirstName} ${owner.LastName}`)
        rentalPaymentsDiv.append(rentalPaymentElem);


    }

}

//added this function
function renderPaidRentalPayment(rentalPayments, rentalAgreement, owner){
    
    var rentalPaymentsDiv = $("#paid-rental-payments-div");
    console.log(rentalPaymentsDiv);
    for (let rentalPayment of rentalPayments){
        //console.log(rentalPayments);
        var rentalPaymentElem = $("<div>");
        rentalPaymentElem.text(`$${rentalAgreement.MonthlyRate} paid on ` +
        `${new Date(rentalPayment.PaidDate).toLocaleDateString()} by ${owner.FirstName} ${owner.LastName}`)
        rentalPaymentsDiv.append(rentalPaymentElem);


    }
}