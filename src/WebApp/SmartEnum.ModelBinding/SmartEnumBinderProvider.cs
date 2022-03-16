﻿using Ardalis.SmartEnum;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace WebApp.SmartEnum.ModelBinding;

public class SmartEnumBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        if (TypeUtil.IsDerived(context.Metadata.ModelType, typeof(SmartEnum<,>)))
            return new BinderTypeModelBinder(typeof(SmartEnumModelBinder));

        return null;
    }
}
