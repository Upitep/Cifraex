using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cifraex
{
    public class HideFieldsInPostRequestOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Кастомизация для CreateUser
            if (operation.OperationId != null && operation.OperationId.Equals("CreateUser"))
            {
                CustomizeCreateUserOperation(operation);
            }
            else if (operation.OperationId != null && operation.OperationId.Equals("UpdateUser"))
            {
                CustomizeUpdateUserOperation(operation);
            }

            switch (operation.OperationId)
            {
                case "GetAllUsers":
                    AddOperationDetails(operation, "Получение списка всех пользователей", "Возвращает список всех пользователей", StatusCodes.Status200OK, "Список пользователей");
                    break;
                case "GetUserById":
                    AddOperationDetails(operation, "Получение информации о пользователе по его идентификатору", "Возвращает пользователя, если он найден", StatusCodes.Status200OK, "Информация о пользователе");
                    AddParameterDescription(operation, "id", "Идентификатор пользователя");
                    AddResponseDescription(operation, StatusCodes.Status404NotFound, "Пользователь не найден");
                    break;
                case "CreateUser":
                    AddOperationDetails(operation, "Создание нового пользователя", "Создает нового пользователя и возвращает его", StatusCodes.Status201Created, "Созданный пользователь");
                    break;
                case "UpdateUser":
                    AddOperationDetails(operation, "Обновление данных пользователя", "Обновляет информацию о пользователе", StatusCodes.Status204NoContent, "Обновленные данные пользователя");
                    AddParameterDescription(operation, "id", "Идентификатор пользователя для обновления");
                    AddParameterDescription(operation, "updateUser", "Обновленные данные пользователя");
                    AddResponseDescription(operation, StatusCodes.Status404NotFound, "Пользователь не найден");
                    break;
                case "DeleteUser":
                    AddOperationDetails(operation, "Удаление пользователя", "Удаляет пользователя из системы", StatusCodes.Status200OK, "Успешное удаление пользователя");
                    AddParameterDescription(operation, "id", "Идентификатор пользователя для удаления");
                    AddResponseDescription(operation, StatusCodes.Status404NotFound, "Пользователь не найден");
                    break;
                case "UpdateUserBalance":
                    AddOperationDetails(operation, "Обновление баланса пользователя", "Обновляет баланс пользователя", StatusCodes.Status200OK, "Успешное обновление баланса");
                    AddParameterDescription(operation, "id", "Идентификатор пользователя для обновления баланса");
                    AddParameterDescription(operation, "balanceUpdate", "Информация об обновлении баланса");
                    AddResponseDescription(operation, StatusCodes.Status404NotFound, "Пользователь не найден");
                    break;
                case "GetAllCurrencies":
                    AddOperationDetails(operation, "Получение списка всех валют", "Возвращает список всех доступных валют", StatusCodes.Status200OK, "Список валют");
                    break;
                case "GetCurrencyByCode":
                    AddOperationDetails(operation, "Получение информации о валюте по коду", "Возвращает детальную информацию о валюте по её коду", StatusCodes.Status200OK, "Информация о валюте");
                    AddParameterDescription(operation, "code", "Уникальный код валюты");
                    AddResponseDescription(operation, StatusCodes.Status404NotFound, "Валюта не найдена");
                    break;
                case "CreateCurrency":
                    AddOperationDetails(operation, "Создание новой валюты", "Создает новую валюту и возвращает информацию о ней", StatusCodes.Status201Created, "Созданная валюта");
                    break;
                case "UpdateCurrency":
                    AddOperationDetails(operation, "Обновление информации о валюте", "Обновляет информацию о существующей валюте", StatusCodes.Status204NoContent, "Обновленная информация о валюте");
                    AddParameterDescription(operation, "code", "Уникальный код валюты для обновления");
                    AddResponseDescription(operation, StatusCodes.Status404NotFound, "Валюта не найдена");
                    break;
                case "DeleteCurrency":
                    AddOperationDetails(operation, "Удаление валюты", "Удаляет валюту по её коду", StatusCodes.Status200OK, "Успешное удаление валюты");
                    AddParameterDescription(operation, "code", "Уникальный код валюты для удаления");
                    AddResponseDescription(operation, StatusCodes.Status404NotFound, "Валюта не найдена");
                    break;
                case "ExchangeCurrency":
                    AddOperationDetails(operation,
                        "Обмен валюты",
                        "Выполняет обмен валюты согласно указанным параметрам. Включает в себя идентификатор пользователя, исходную и целевую валюты, сумму для обмена, курс обмена и комиссию.",
                        StatusCodes.Status200OK,
                        "Обмен валюты выполнен успешно");
                    AddParameterDescription(operation, "UserId", "Идентификатор пользователя, для которого выполняется обмен");
                    AddParameterDescription(operation, "FromCurrency", "Код валюты, из которой выполняется обмен");
                    AddParameterDescription(operation, "ToCurrency", "Код валюты, в которую выполняется обмен");
                    AddParameterDescription(operation, "Amount", "Сумма в исходной валюте для обмена");
                    AddParameterDescription(operation, "ExchangeRate", "Курс обмена между валютами");
                    AddParameterDescription(operation, "CommissionRate", "Комиссионные за обмен в процентах");
                    AddResponseDescription(operation, StatusCodes.Status400BadRequest, "Ошибка в запросе или в процессе обмена");
                    break;
            }
        }
        private void CustomizeCreateUserOperation(OpenApiOperation operation)
        {
            // Создаем новую схему, которая будет использоваться только для этой операции
            var newSchema = new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["name"] = new OpenApiSchema { Type = "string" },
                },
                Required = new HashSet<string> { "name" } // Указываем обязательные поля, если они есть
            };

            operation.RequestBody.Reference = null;
            // Задаем новую схему для RequestBody текущей операции
            operation.RequestBody.Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = newSchema
                }
            };
        }

        private void CustomizeUpdateUserOperation(OpenApiOperation operation)
        {
            // Создаем новую схему, которая будет использоваться только для этой операции
            var newSchema = new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["userId"] = new OpenApiSchema { Type = "integer", Format = "int32"},
                    ["name"] = new OpenApiSchema { Type = "string" },
                },
                Required = new HashSet<string> { "userId" } // Указываем обязательные поля, если они есть
            };

            operation.RequestBody.Reference = null;
            // Задаем новую схему для RequestBody текущей операции
            operation.RequestBody.Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = newSchema
                }
            };
        }
        private void AddOperationDetails(OpenApiOperation operation, string summary, string description, int successStatusCode, string successDescription)
        {
            operation.Summary = summary;
            operation.Description = description;
            AddResponseDescription(operation, successStatusCode, successDescription);
        }

        private void AddParameterDescription(OpenApiOperation operation, string paramName, string description)
        {
            var parameter = operation.Parameters.FirstOrDefault(p => p.Name == paramName);
            if (parameter != null)
            {
                parameter.Description = description;
            }
        }

        private void AddResponseDescription(OpenApiOperation operation, int statusCode, string description)
        {
            if (operation.Responses.ContainsKey(statusCode.ToString()))
            {
                operation.Responses[statusCode.ToString()].Description = description;
            }
            else
            {
                operation.Responses.Add(statusCode.ToString(), new OpenApiResponse { Description = description });
            }
        }
    }
}
