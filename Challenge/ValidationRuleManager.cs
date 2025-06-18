using System.ComponentModel.Design;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

public class ValidationRule
{
    public string? ClassName { get; set; }
    public string? FieldName { get; set; }
    public Func<object, bool>? Condition { get; set; }
    public bool IsRequired { get; set; }
}

public class ValidationRuleManager
{
    private List<ValidationRule> rules = new List<ValidationRule>();

    public void SetRequired(string className, string fieldName, Func<object, bool> condition, bool isRequired)
    {
        rules.Add(new ValidationRule
        {
            ClassName = className,
            FieldName = fieldName,
            Condition = condition,
            IsRequired = isRequired
        });
    }

    public List<string> Validate(object obj)
    {
        var errors = new List<string>();
        var className = obj.GetType().Name;

        foreach (var rule in rules.Where(r => r.ClassName == className))
        {
            if (rule.Condition(obj))
            {
                var value = obj.GetType().GetProperty(rule.FieldName)?.GetValue(obj);
                if (rule.IsRequired && (value == null || string.IsNullOrEmpty(value.ToString())))
                {
                    errors.Add($"{rule.FieldName} is required when rule applies.");
                }
            }
        }

        return errors;
    }
}
