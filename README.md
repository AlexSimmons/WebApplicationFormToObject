# .Net Web Application HTML Form to Object Mapper

This project provides a simple, efficient way to map .Net HTML forms to objects and vice versa. It provides two core functionalities:
- Loading an object's values into an HTML form.
- Saving HTML form data back into an object.

This utility is developed as a lightweight library with no external dependencies, making it easy to integrate into any .Net application.

## Features

- **LoadObjectToForm**: Load the properties of an object into an HTML form.
- **SaveFormToObject**: Save the data from an HTML form back into an object.

## Usage

Below are examples of how you can use the provided methods:

### LoadObjectToForm

Use the `LoadObjectToForm` method to populate an HTML form with the properties of an object:

```csharp
HtmlForm form = new HtmlForm();
MyObject objectToLoad = new MyObject { Name = "Test", Age = 25 };

WebApplicationFormToObject.Mapper.LoadObjectToForm(form, objectToLoad);
```

In this example, `objectToLoad` is an instance of `MyObject` which is being loaded into `form`. The method has two optional parameters:
- `lockFields` (default `false`): If `true`, the fields will be locked (non-editable) after loading.
- `ignoreStrings` (default `false`): If `true`, string properties of the object will be ignored when loading.

### SaveFormToObject

Use the `SaveFormToObject` method to extract the data from an HTML form and save it into an object:

```csharp
HtmlForm form = new HtmlForm();
MyObject objectToSave = new MyObject();

bool isSuccess = WebApplicationFormToObject.Mapper.SaveFormToObject(form, objectToSave);
```

In this example, the data from `form` is being saved into `objectToSave`. The method has one optional parameter `allowNullableParameter` (default `false`), that indicates whether null values are allowed to be set for the properties of the object. The method returns a boolean value indicating the success of the operation.

## Prerequisites

This project requires .Net 4.x or higher.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.
