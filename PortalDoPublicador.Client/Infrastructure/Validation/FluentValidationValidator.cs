using FluentValidation;
using FluentValidation.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace PortalDoPublicador.Client.Infrastructure.Validation;

public class FluentValidationValidator : ComponentBase, IDisposable
{
    [CascadingParameter]
    private EditContext? CurrentEditContext { get; set; }

    [Inject]
    private IServiceProvider ServiceProvider { get; set; } = default!;

    private ValidationMessageStore? _messageStore;

    protected override void OnInitialized()
    {
        if (CurrentEditContext is null)
        {
            throw new InvalidOperationException($"{nameof(FluentValidationValidator)} deve estar dentro de um <EditForm>.");
        }

        _messageStore = new(CurrentEditContext);

        CurrentEditContext.OnValidationRequested += HandleValidationRequested;
        CurrentEditContext.OnFieldChanged += HandleFieldChanged;
    }

    private async void HandleValidationRequested(object? sender, ValidationRequestedEventArgs e) =>
        await ValidateModelAsync();

    private async void HandleFieldChanged(object? sender, FieldChangedEventArgs e) =>
        await ValidateFieldAsync(e.FieldIdentifier);

    private async Task ValidateModelAsync()
    {
        if (CurrentEditContext is null)
        {
            return;
        }

        _messageStore?.Clear();
        var model = CurrentEditContext.Model;
        var validatorType = typeof(IValidator<>).MakeGenericType(model.GetType());

        if (ServiceProvider.GetService(validatorType) is IValidator validator)
        {
            try
            {
                var validationContextType = typeof(ValidationContext<>).MakeGenericType(model.GetType());
                var context = (IValidationContext)Activator.CreateInstance(validationContextType, model)!;

                var result = await validator.ValidateAsync(context);
                foreach (var error in result.Errors)
                {
                    var fieldIdentifier = new FieldIdentifier(model, error.PropertyName);
                    _messageStore?.Add(fieldIdentifier, error.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FluentValidation] Erro ao validar modelo: {ex.Message}");
            }
        }

        CurrentEditContext.NotifyValidationStateChanged();
    }

    private async Task ValidateFieldAsync(FieldIdentifier fieldIdentifier)
    {
        if (CurrentEditContext is null)
        {
            return;
        }

        _messageStore?.Clear(fieldIdentifier);
        var model = CurrentEditContext.Model;
        var validatorType = typeof(IValidator<>).MakeGenericType(model.GetType());

        if (ServiceProvider.GetService(validatorType) is IValidator validator)
        {
            try
            {
                var validationContextType = typeof(ValidationContext<>).MakeGenericType(model.GetType());
                var selector = ValidatorOptions.Global.ValidatorSelectors.MemberNameValidatorSelectorFactory([fieldIdentifier.FieldName]);
                var context = (IValidationContext)Activator.CreateInstance(validationContextType, [model, new PropertyChain(), selector])!;

                var result = await validator.ValidateAsync(context);
                foreach (var error in result.Errors.Where(e => e.PropertyName == fieldIdentifier.FieldName))
                {
                    _messageStore?.Add(fieldIdentifier, error.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FluentValidation] Erro ao validar campo '{fieldIdentifier.FieldName}': {ex.Message}");
            }
        }

        CurrentEditContext.NotifyValidationStateChanged();
    }

    public void Dispose()
    {
        if (CurrentEditContext is not null)
        {
            CurrentEditContext.OnValidationRequested -= HandleValidationRequested;
            CurrentEditContext.OnFieldChanged -= HandleFieldChanged;
        }
    }
}
