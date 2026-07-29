using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using FluentValidation;

namespace PortalDoPublicador.Client.Infrastructure.Validation;

public class FluentValidationValidator : ComponentBase
{
    [CascadingParameter]
    private EditContext? CurrentEditContext { get; set; }

    [Inject]
    private IServiceProvider ServiceProvider { get; set; } = default!;

    private ValidationMessageStore? _messageStore;

    protected override void OnInitialized()
    {
        if (CurrentEditContext == null)
            throw new InvalidOperationException($"{nameof(FluentValidationValidator)} deve estar dentro de um <EditForm>.");

        _messageStore = new ValidationMessageStore(CurrentEditContext);

        CurrentEditContext.OnValidationRequested += (s, e) => ValidateModel();
        CurrentEditContext.OnFieldChanged += (s, e) => ValidateField(e.FieldIdentifier);
    }

    private void ValidateModel()
    {
        _messageStore?.Clear();
        var model = CurrentEditContext!.Model;
        var validatorType = typeof(IValidator<>).MakeGenericType(model.GetType());
        if (ServiceProvider.GetService(validatorType) is IValidator validator)
        {
            var validationContextType = typeof(ValidationContext<>).MakeGenericType(model.GetType());
            var context = (IValidationContext)Activator.CreateInstance(validationContextType, model)!;
            
            var result = validator.Validate(context);
            foreach (var error in result.Errors)
            {
                var fieldIdentifier = new FieldIdentifier(model, error.PropertyName);
                _messageStore?.Add(fieldIdentifier, error.ErrorMessage);
            }
        }
        CurrentEditContext.NotifyValidationStateChanged();
    }

    private void ValidateField(FieldIdentifier fieldIdentifier)
    {
        _messageStore?.Clear(fieldIdentifier);
        var model = CurrentEditContext!.Model;
        var validatorType = typeof(IValidator<>).MakeGenericType(model.GetType());
        if (ServiceProvider.GetService(validatorType) is IValidator validator)
        {
            var validationContextType = typeof(ValidationContext<>).MakeGenericType(model.GetType());
            var context = (IValidationContext)Activator.CreateInstance(validationContextType, model)!;
            
            var result = validator.Validate(context);
            foreach (var error in result.Errors.Where(e => e.PropertyName == fieldIdentifier.FieldName))
            {
                _messageStore?.Add(fieldIdentifier, error.ErrorMessage);
            }
        }
        CurrentEditContext.NotifyValidationStateChanged();
    }
}
