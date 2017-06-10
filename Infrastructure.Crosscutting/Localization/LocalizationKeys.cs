namespace NLayerApp.Infrastructure.Crosscutting.Localization
{
    public class LocalizationKeys
    {
        public enum Infrastructure
        {
            info_CannotAddNullEntity,
            info_CannotModifyNullEntity,
            info_CannotRemoveNullEntity,
            info_CannotTrackNullEntity,

            exception_NotMapFoundForTypeAdapter,
            exception_RegisterTypeMapConfigurationElementInvalidTypeValue,
            exception_RegisterTypesMapConfigurationInvalidType,

            exception_InvalidEnumeratedType
        };
        public enum Domain
        {
            validation_PropertyIsEmptyOrNull,
            validation_PropertyOutOfRange,

            exception_BankAccountCannotDeposit,
            exception_BankAccountCannotWithdraw,
            exception_BankAccountInvalidWithdrawAmount,
            exception_CannotAssociateTransientOrNullCountry,
            exception_CannotAssociateTransientOrNullCustomer,
            exception_CannotAssociateTransientOrNullProduct,
            exception_CannotTransferMoneyWhenFromIsTheSameAsTo,
            exception_InvalidDataForOrderLine,
            exception_OrderNumberSpecificationInvalidOrderNumberPattern,
            messages_TransactionFromMessage,
            messages_TransactionToMessage,
            validation_BankAccountBankAccountNumberCannotBeNull,
            validation_BankAccountBankCheckDigitsCannotBeNull,
            validation_BankAccountBankNationalBankCodeCannotBeNull,
            validation_BankAccountBankOfficeNumberCannotBeNull,
            validation_BankAccountCustomerIdIsEmpty,
            validation_BankAccountNumberCannotBeNull,
            validation_CountryCountryISOCodeCannotBeNull,
            validation_CountryCountryNameCannotBeNull,
            validation_CustomerCountryIdCannotBeEmpty,
            validation_CustomerFirstNameCannotBeNull,
            validation_CustomerLastNameCannotBeBull,
            validation_OrderCustomerIdCannotBenull,
            validation_OrderLineAmountLessThanOne,
            validation_OrderLineDiscountCannotBeLessThanZeroOrGreaterThanOne,
            validation_OrderLineOrderIdIsEmpty, 
            validation_OrderLineProductIdCannotBeNull,
            validation_OrderLineUnitPriceLessThanZero,
            validation_ProductAmountLessThanZero,
            validation_ProductDescriptionCannotBeNullOrEmpty, 
            validation_ProductTitleCannotBeNullOrEmpty, 
            validation_ProductUnitPriceLessThanZero,


        }
    public enum Application
        {
            validation_BlogDto_Empty_Name,
            validation_BlogDto_Empty_Url,
            validation_BlogDto_Invalid_Rating,

            validation_PostDto_Empty_Title,
            validation_PostDto_Empty_Content,
            validation_PostDto_Invalid_BlogId,

            validation_No_Records_Found_Error,
            validation_Validation_Error,
            validation_Null_Parameters_Error,
            validation_BadRequest,
            validation_Exception,

            exception_ApplicationValidationExceptionDefaultMessage,

            error_CannotPerformTransferInvalidAccounts,
            exception_InvalidCountryIdentifier,
            exception_InvalidCustomerIdentifier,
            exception_InvalidPageIndexOrPageCount,
            info_OrderTotalIsGreaterCustomerCredit,
            warning_CannotAddBookWithNullInformation,
            warning_CannotAddCustomerWithEmptyInformation,
            warning_CannotAddNotValidatedBankAccount,
            warning_CannotAddNullBankAccountOrInvalidCustomer,
            warning_CannotAddOrderWithNullInformation,
            warning_CannotAddSoftwareWithNullInformation,
            warning_CannotCreateBankAccountForInvalidCustomer,
            warning_CannotCreateBankAccountForNonExistingCustomer,
            warning_CannotCreateOrderForNonExistingCustomer,
            warning_CannotGetActivitiesForInvalidOrNotExistingBankAccount,
            warning_CannotGetOrdersFromEmptyCustomerId,
            warning_CannotLockBankAccountWithEmptyIdentifier,
            warning_CannotLockNonExistingBankAccount,
            warning_CannotRemoveCustomerWithEmptyId,
            warning_CannotRemoveNonExistingCustomer,
            warning_CannotUpdateCustomerWithEmptyInformation,
            warning_CannotUpdateNonExistingCustomer,
            warning_InvalidArgumentForFindCustomer,
            warning_InvalidArgumentForFindOrders,
            warning_InvalidArgumentForFindProducts,
            warning_InvalidArgumentsForFindCountries,
            warning_InvalidArgumentsForFindCustomers
        }

public enum Distributed_Services
        {
            info_OnExecuting,
            info_Parameter,
            info_OnExecuted,
        }
    }
}
