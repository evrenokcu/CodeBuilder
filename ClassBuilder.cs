using CodeBuilder.Implementations;

namespace CodeBuilder;

public static class ClassBuilder
{
    public static string CreateField(string fieldName, string type, string fieldValue = "")
        => $"{type} {fieldName} {$" = {fieldValue}".IfDefined(fieldValue)};";

    public static string CreateProperty(string propertyName, string type, string propertyValue = "",
        AccessModifier accessModifier = AccessModifier.Private,
        PropertyCodeBuilder.PropertySetAccessModifier modifier = PropertyCodeBuilder.PropertySetAccessModifier.NoSet,
        bool isConstant = false, bool isStatic = false, bool isReadOnly = false, bool isNullable = false,
        bool isOverride = false)
        =>
            $"{GetAccessModifier(accessModifier)}{" override".IfTrue(isOverride)}{" const".IfTrue(isConstant)}{" static".IfTrue(isStatic)}{" readonly".IfTrue(isReadOnly)} {type}{"?".IfTrue(isNullable)} {propertyName} {$"{{ get; {GetPropertySetModifier(modifier)}}}".IfTrue(!isConstant)} {propertyValue.IfDefined(() => $" = {propertyValue};")}";

    public static string IfDefined(this string str, Func<string>? func = null)
        => string.IsNullOrWhiteSpace(str)
            ? string.Empty
            : func == null
                ? str
                : func.Invoke();

    public static string IfDefined(this string str, string toCheck)
        => string.IsNullOrWhiteSpace(toCheck)
            ? string.Empty
            : str;

    public static string IfTrue(this string str, bool condition)
        => condition ? str : string.Empty;


    public static string GetPropertySetModifier(PropertyCodeBuilder.PropertySetAccessModifier modifier) =>
        modifier switch
        {
            PropertyCodeBuilder.PropertySetAccessModifier.NoSet => string.Empty,
            PropertyCodeBuilder.PropertySetAccessModifier.Public => " set;",
            PropertyCodeBuilder.PropertySetAccessModifier.Init => " init set;",
            PropertyCodeBuilder.PropertySetAccessModifier.Private => " private set",
            _ => throw new ArgumentException("Invalid enum value for ", nameof(modifier))
        };

    public static string GetAccessModifier(AccessModifier modifier) =>
        modifier switch
        {
            AccessModifier.Public => "public",
            AccessModifier.Private => "private",
            AccessModifier.Internal => "internal",
            AccessModifier.Protected => "protected",
            AccessModifier.None => "",
            _ => throw new ArgumentException("Invalid enum value for ", nameof(modifier))
        };

    public static string CreateFunctionHeader(string functionName, IEnumerable<string>? parameters,
        AccessModifier accessModifier,
        bool isStatic = false, string returnType = "", string baseCall = "", string postFix = "",
        bool @override = false, bool isStatement = false, bool isVirtual = false, bool isAsync = false) =>
        $"{GetAccessModifier(accessModifier)} {"static ".IfTrue(isStatic)}{"virtual ".IfTrue(isVirtual)}{"override ".IfTrue(@override)}{"async ".IfTrue(isAsync)}{HandleAsync(returnType, isAsync)} {functionName}({parameters?.JoinStrings(", ")}) {baseCall.IfDefined(() => $" : {baseCall}")} {postFix}{";".IfTrue(isStatement)}";

    private static string HandleAsync(string returnType, bool isAsync)
    {
        return (isAsync)
            ? (string.IsNullOrWhiteSpace(returnType)) ? "Task" : returnType.Wrap("Task<", ">")
            : returnType;
    }

    public static string CreatePublicClassHeader(string className, bool isPartial = false, bool isStatic = false,
        string baseType = "") =>
        $"public {"static ".IfTrue(isStatic)}{"partial ".IfTrue(isPartial)}class {className} {baseType.IfDefined(() => $": {baseType}")}";

    public static string CreateEnumHeader(string enumName) => $"public enum {enumName}";

    public static string CreateAssignment(string prefix, string variableName, string value, string classType = "",
        string operation = "=", bool isVarAssignment = false, bool isStatement = true,
        bool isCommaInsteadOfSemicolon = false, bool isThisAssignment = false, bool isThrowExceptionIfNull = false,
        string typeCastClass = "")
    {
        return prefix + "var ".IfTrue(isVarAssignment) + (classType == string.Empty ? string.Empty : classType + " ") +
               "this.".IfTrue(isThisAssignment) + variableName + operation.Wrap(" ") +
               $"{typeCastClass.IfDefined(() => typeCastClass /*.WrapParenthesis()*/)}" + value +
               $" ?? throw new ArgumentNullException(nameof({value}))".IfTrue(isThrowExceptionIfNull) +
               ";".IfTrue(isStatement);
    }

    public static string CreateThisAssignment(string prefix, string variableName, string value = "")
    {
        return CreateAssignment(prefix + "this.", variableName, value == string.Empty ? variableName : value);
    }

    public static string CreateVarAssignment(string prefix, string variableName, string value)
    {
        return CreateAssignment(prefix + "var ", variableName, value);
    }

    public static string CreateFunctionCallStatement(string prefix, string functionName,
        IEnumerable<string>? parameters = null, bool isStatement = false, string postFix = "",
        string genericType = "",
        string instanceOfClass = "", bool isInitialization = false, bool isAwait = false)
    {
        return CreateFunctionCall(prefix, functionName, parameters, isStatement, postFix, genericType, instanceOfClass,
            isInitialization, isAwait);
    }

    public static string CreateLambdaStatement(string mapsTo, bool isStatement = false,
        List<string>? lambdaParameters = null)
        =>
            $"({string.Join(", ", lambdaParameters ?? new List<string>()).IfDefined()})=>{mapsTo}{";".IfTrue(isStatement)}";


    public static string CreateFunctionCall(string prefix, string functionName, IEnumerable<string>? parameters,
        bool isStatement, string postFix = "", string genericType = "", string instanceOfClass = "",
        bool isInitialization = false, bool isAwait = false) =>
        $"{prefix}{"await ".IfTrue(isAwait)}{instanceOfClass.IfDefined(() => $"{instanceOfClass}.")}{functionName}{genericType.IfDefined(() => $"<{genericType}>")}({parameters?.JoinStrings(", ")}){postFix}{";".IfTrue(isStatement && !isInitialization)}";

    public static string CreateAttributeWithValue(string value) =>
        CreateAttribute(value, string.Empty);

    public static string CreateAttribute(string name, IList<string> attributeValues) =>
        CreateAttribute(name, attributeValues.IfNotEmpty(() => attributeValues.JoinStrings(", ")));

    public static string CreateAttribute(string name, string attributeValue) =>
        $"[{name}{(string.IsNullOrWhiteSpace(attributeValue) ? string.Empty : attributeValue.Wrap("(", ")"))}]";

    public static string CreateNamespace(string name) => $"namespace {name}";

    internal static string CreateReturn(string value) => $"return {value};";

    public static string PropertyAccess(string variableName, string propertyName) => $"{variableName}.{propertyName}";


    public static string GenericClassOf(string genericClass, string genericType) => $"{genericClass}<{genericType}>";

    public static string CreateParameters(string[] parameters) => parameters.JoinStrings(", ");

    public static string CreateInterfaceHeader(string name, string baseType = "") =>
        $"public interface {name} {baseType.IfDefined(() => $": {baseType}")}";

    public static string CreateIfStatement(string condition) => $"if ({condition})";

    public static string CreateCondition(string condition, bool isNegate) => $"{"!".IfTrue(isNegate)}{condition}";

    public static string NewConstructor() => "new ()";

    public static string Nullable(string nullableVariable) => $"{nullableVariable}?";

    public static string EmptyString() => "string.Empty";
    public static string GetResultTypeOf(string type) => type == string.Empty ? "Result" : $"Result<{type}>";

    public static string GetVariableNameOfResult(string type) => type == string.Empty ? "result" : type.ToCamelCase();

    public static string ToCamelCase(this string str)
    {
        if (string.IsNullOrWhiteSpace(str)) throw new ArgumentOutOfRangeException(nameof(str));
        return char.ToLowerInvariant(str[0]) + str.Substring(1);
    }

    public static string CreateArgumentNullCheck(string name)
        => $"_ = {name} ?? throw new ArgumentNullException(nameof({name}));";

    public static string CreateComment(string name)
        => $"//{name}";
}