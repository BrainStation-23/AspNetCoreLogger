namespace WebApp.Core.Exceptions
{
    public class ApplicationExceptionMessage
    {
        #region Common Message
        public const string NullObjectReference = "Null object reference error occured.";
        public const string SavedSuccessfully = "Saved successfully.";
        public const string UpdatedSuccessfully = "Updated successfully.";
        public const string DeletedSuccessfully = "Deleted successfully.";
        public const string SaveFailed = "Save failed.";
        public const string UpdateFailed = "Update failed.";
        public const string DeleteFailed = "Delete failed.";
        public const string DeleteOperationNotValid = "Delete operation not valid as this entry is used in {0}.";
        public const string NotFound = "{0} not found.";
        public const string Required = "{0} required.";
        public const string DateValidation = "{0} can't be greater than {1}.";
        public const string DateRequiredValidation = "Minimum one {0} & {1} is required.";
        public const string QueueEmpty = "No pending data found for entry.";
        public const string GetNextInvalid = "Get next request is not valid.";
        public const string AlreadyExists = "{0} already exists.";
        public const string PleaseEntryUniqueName = "Please enter a unique {0} name.";
        public const string NotValid = "{0} is not valid";
        public const string NotAvailable = "{0} not available";
        public const string InvalidEffectiveDateOrCurrentDateOrInvalidDate = "Invalid date input. Please check effective date, current date and inactive date.";
        public const string InvalidValue = "Invalid value.";
        public const string NotExist = "{0} not exist";
        public const string PolicyCertificateInvalid = "Policy Number and certicate number combination is not valid.";
        public const string ContainsNonActive = "{0} contains non-active {1}.";
        public const string SendToQueueRequestNotValid = "Send to {0} Queue Request is not valid.";
        public const string ImportedSuccessfully = "{0}({1} data['s]) imported successfully.";
        public const string AlreadyExistsImport = "{0} already exists. Please correct and import again. Exist data: {1}";
        public const string AlreadyAssigned = "{0} already assigned";
        public const string NotValidInCerificate = "{0} not valid in certificate number {1}";
        public const string CouldNotFetchFromLifeLine = "Data could not be fetched from Lifeline.";
        public const string SomethingWentWrong = "Something went wrong";
        public const string OrganizationExistsinGroupPolicy = "This Organization exists in Group Policy";
        public const string DateOverlapped = "Date overlapped!";
        public const string CashValueNoDataFoundInLifeline = "Cash Value - No Data Found in Lifeline";
        public const string SurrenderValueNoDataFoundInLifeline = "Surrender Value - No Data Found in Lifeline";
        public const string BenefitCalculationNoDataFoundInLifeline = "Benefit Calculation - No Data Found in Lifeline";
        public const string NPolicyInfoNoDataFoundInLifeline = "N Policy Data - No Data Found in Lifeline";
        public const string LoanAplValueNoDataFoundInLifeline = "Loan Apl Value - No Data Found in Lifeline";
        public const string LoanQuotationValueNoDataFoundInLifeline = "Loan Quotation Value - No Data Found in Lifeline";

        #endregion
    }
}
