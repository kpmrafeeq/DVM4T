﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DVM4T.Attributes;
using Dynamic = DD4T.ContentModel;
using DVM4T.Contracts;
using DVM4T.Reflection;
using DD4T.Mvc.Html;
using System.Web.Mvc;
using DVM4T.Core;
using DVM4T.Exceptions;
using DD4T.ContentModel;
using System.Reflection;
using System.Collections;

namespace DVM4T.DD4T.Attributes
{
    /// <summary>
    /// An Attribute for a Property representing the Link Resolved URL for a Linked or Multimedia Component
    /// </summary>
    /// <remarks>Uses the default DD4T GetResolvedUrl helper method</remarks>
    public class ResolvedUrlFieldAttribute : FieldAttributeBase
    {
        //public ResolvedUrlFieldAttribute(string fieldName) : base(fieldName) { }
        public override IEnumerable GetFieldValues(IFieldData field, IModelProperty property, ITemplateData template, IViewModelFactory factory)
        {
            return field.Values.Cast<Dynamic.IComponent>()
                .Select(x => x.GetResolvedUrl());
        }

        public override Type ExpectedReturnType
        {
            get { return AllowMultipleValues ? typeof(IList<string>) : typeof(string); }
        }
    }

    /// <summary>
    /// A Multimedia component field
    /// </summary>
    public class MultimediaFieldAttribute : FieldAttributeBase
    {
        //public MultimediaFieldAttribute(string fieldName) : base(fieldName) { }
        public override IEnumerable GetFieldValues(IFieldData field, IModelProperty property, ITemplateData template, IViewModelFactory factory)
        {
            return field.Values.Cast<Dynamic.IComponent>().Select(x => x.Multimedia);
        }

        public override Type ExpectedReturnType
        {
            get { return AllowMultipleValues ? typeof(IList<Dynamic.IMultimedia>) : typeof(Dynamic.IMultimedia); }
        }
    }
    /// <summary>
    /// A text field
    /// </summary>
    public class TextFieldAttribute : FieldAttributeBase, ICanBeBoolean
    {
        public override IEnumerable GetFieldValues(IFieldData field, IModelProperty property, ITemplateData template, IViewModelFactory factory)
        {
            IEnumerable fieldValue = null;
            var values = field.Values.Cast<string>();
            if (IsBooleanValue)
                fieldValue = values.Select(v => { bool b; return bool.TryParse(v, out b) && b; });
            else fieldValue = values;

            return fieldValue;
        }

        /// <summary>
        /// Set to true to parse the text into a boolean value.
        /// </summary>
        public bool IsBooleanValue { get; set; }
        public override Type ExpectedReturnType
        {
            get
            {
                if (AllowMultipleValues)
                    return IsBooleanValue ? typeof(IList<bool>) : typeof(IList<string>);
                else return IsBooleanValue ? typeof(bool) : typeof(string);
            }
        }
    }
    /// <summary>
    /// A Rich Text field
    /// </summary>
    public class RichTextFieldAttribute : FieldAttributeBase
    {
        //public RichTextFieldAttribute(string fieldName) : base(fieldName) { }
        public override IEnumerable GetFieldValues(IFieldData field, IModelProperty property, ITemplateData template, IViewModelFactory factory)
        {
            return field.Values.Cast<string>()
                .Select(v => v.ResolveRichText()); //Hidden dependency on DD4T implementation
        }

        public override Type ExpectedReturnType
        {
            get { return AllowMultipleValues ? typeof(IList<MvcHtmlString>) : typeof(MvcHtmlString); }
        }
    }
    /// <summary>
    /// A Number field
    /// </summary>
    public class NumberFieldAttribute : FieldAttributeBase
    {
        //public NumberFieldAttribute(string fieldName) : base(fieldName) { }
        public override IEnumerable GetFieldValues(IFieldData field, IModelProperty property, ITemplateData template, IViewModelFactory factory)
        {
            return field.Values.Cast<double>();
        }

        public override Type ExpectedReturnType
        {
            get { return AllowMultipleValues ? typeof(IList<double>) : typeof(double); }
        }

    }
    /// <summary>
    /// A Date/Time field
    /// </summary>
    public class DateFieldAttribute : FieldAttributeBase
    {
        //public DateFieldAttribute(string fieldName) : base(fieldName) { }
        public override IEnumerable GetFieldValues(IFieldData field, IModelProperty property, ITemplateData template, IViewModelFactory factory)
        {
            return field.Values.Cast<DateTime>();
        }

        public override Type ExpectedReturnType
        {
            get { return AllowMultipleValues ? typeof(IList<DateTime>) : typeof(DateTime); }
        }
    }
    /// <summary>
    /// The Key of a Keyword field. 
    /// </summary>
    public class KeywordKeyFieldAttribute : FieldAttributeBase, ICanBeBoolean
    {
        /// <summary>
        /// The Key of a Keyword field.
        /// </summary>
        /// <param name="fieldName">Tridion schema field name</param>
        //public KeywordKeyFieldAttribute(string fieldName) : base(fieldName) { }
        public override IEnumerable GetFieldValues(IFieldData field, IModelProperty property, ITemplateData template, IViewModelFactory factory)
        {
            IEnumerable value = null;
            var values = field.Values.Cast<Dynamic.IKeyword>();
            if (IsBooleanValue)
                value = values.Select(k => { bool b; return bool.TryParse(k.Key, out b) && b; });
            else value = values.Select(k => k.Key);
            return value;
        }

        /// <summary>
        /// Set to true to parse the Keyword Key into a boolean value.
        /// </summary>
        public bool IsBooleanValue { get; set; }
        public override Type ExpectedReturnType
        {
            get
            {
                if (AllowMultipleValues)
                    return IsBooleanValue ? typeof(IList<bool>) : typeof(IList<string>);
                else return IsBooleanValue ? typeof(bool) : typeof(string);
            }
        }
    }
    /// <summary>
    /// The Key of a Keyword as a number
    /// </summary>
    public class NumericKeywordKeyFieldAttribute : FieldAttributeBase
    {
        //public NumericKeywordKeyFieldAttribute(string fieldName) : base(fieldName) { }
        public override IEnumerable GetFieldValues(IFieldData field, IModelProperty property, ITemplateData template, IViewModelFactory factory)
        {
            return field.Values.Cast<Dynamic.IKeyword>()
                .Select(k => { double i; double.TryParse(k.Key, out i); return i; });
        }

        public override Type ExpectedReturnType
        {
            get
            {
                return AllowMultipleValues ? typeof(IList<double>) : typeof(double);
            }
        }
    }
    /// <summary>
    /// The URL of the Multimedia data of the view model
    /// </summary>
    public class MultimediaUrlAttribute : ComponentAttributeBase
    {
        public override IEnumerable GetPropertyValues(IComponentData component, IModelProperty property, ITemplateData template, IViewModelFactory factory)
        {
            IEnumerable result = null;
            if (component != null && component.MultimediaData != null)
            {
                result = new string[] { component.MultimediaData.Url };
            }
            return result;
        }

        public override Type ExpectedReturnType
        {
            get { return typeof(String); }
        }
    }
    /// <summary>
    /// The Multimedia data of the view model
    /// </summary>
    public class MultimediaAttribute : ComponentAttributeBase
    {
        public override IEnumerable GetPropertyValues(IComponentData component, IModelProperty property, ITemplateData template, IViewModelFactory factory)
        {
            IMultimediaData result = null;
            if (component != null && component.MultimediaData != null)
            {
                result = component.MultimediaData;
            }
            return new IMultimediaData[] { result };
        }

        public override Type ExpectedReturnType
        {
            get { return typeof(IMultimediaData); }
        }
    }
    /// <summary>
    /// The title of the Component (if the view model represents a Component)
    /// </summary>
    public class ComponentTitleAttribute : ComponentAttributeBase
    {
        public override IEnumerable GetPropertyValues(IComponentData component, IModelProperty property, ITemplateData template, IViewModelFactory factory)
        {
            return component == null ? null : new string[] { component.Title };
        }
        public override Type ExpectedReturnType
        {
            get { return typeof(String); }
        }
    }
    /// <summary>
    /// A DD4T IMultimedia object representing the multimedia data of the model
    /// </summary>
    public class DD4TMultimediaAttribute : ComponentAttributeBase
    {
        //Example of using the BaseData object

        public override IEnumerable GetPropertyValues(IComponentData component, IModelProperty property, ITemplateData template, IViewModelFactory factory)
        {
            Dynamic.IMultimedia[] result = null;
            if (component != null && component.MultimediaData != null)
            {
                result = new Dynamic.IMultimedia[] { component.MultimediaData.BaseData as Dynamic.IMultimedia };
            }
            return result;
        }

        public override Type ExpectedReturnType
        {
            get { return typeof(Dynamic.IMultimedia); }
        }
    }
    /// <summary>
    /// Component Presentations filtered by the DD4T CT Metadata "view" field
    /// </summary>
    public class PresentationsByViewAttribute : ComponentPresentationsAttributeBase
    {
        public override System.Collections.IEnumerable GetPresentationValues(IList<IComponentPresentationData> cps, IModelProperty property, IViewModelFactory factory)
        {
            return cps.Where(cp =>
                    {
                        bool result = false;
                        if (cp.Template != null && cp.Template.Metadata != null
                            && cp.Template.Metadata.ContainsKey("view"))
                        {
                            var view = cp.Template.Metadata["view"].Values.Cast<string>().FirstOrDefault();
                            if (view != null && view.StartsWith(ViewPrefix))
                            {
                                result = true;
                            }
                        }
                        return result;
                    })
                    .Select(cp =>
                        {
                            object model = null;
                            if (ComplexTypeMapping != null)
                            {
                                model = factory.BuildMappedModel(cp, ComplexTypeMapping);
                            }
                            else model = factory.BuildViewModel((cp));
                            return model;
                        });
        }
       
        public string ViewPrefix { get; set; }
        public override Type ExpectedReturnType
        {
            get { return typeof(IList<IViewModel>); }
        }
    }



    public class KeywordDataAttribute : ModelPropertyAttributeBase
    {
        public override IEnumerable GetPropertyValues(IViewModelData modelData, IModelProperty property, IViewModelFactory factory)
        {
            IEnumerable result = null;
            if (modelData != null && modelData is IKeywordData)
            {
                result = new IKeywordData[] { modelData as IKeywordData };
            }
            return result;
        }

        public override Type ExpectedReturnType
        {
            get { return typeof(IKeywordData); }
        }
    }

    /// <summary>
    /// Field that is parsed into an Enum
    /// </summary>
    public class EnumFieldAttribute : FieldAttributeBase
    {  
        public override IEnumerable GetFieldValues(IFieldData field, IModelProperty property, ITemplateData template, IViewModelFactory factory)
        {
            var result = new List<object>();
            foreach (var value in field.Values)
            {
                object parsed;
                if (EnumTryParse(property.ModelType, value, out parsed))
                {
                    result.Add(parsed);
                }
            }
            return result;
        }

        public override Type ExpectedReturnType
        {
            get { return AllowMultipleValues ? typeof(IList<Enum>) : typeof(Enum); }
        }

        private bool EnumTryParse(Type enumType, object value, out object parsedEnum)
        {
            bool result = false;
            parsedEnum = null;
            if (value != null)
            {
                try
                {
                    parsedEnum = Enum.Parse(enumType, value.ToString());
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
