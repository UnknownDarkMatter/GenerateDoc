﻿using GenerateDoc.Business.Interfaces;
using GenerateDoc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GenerateDoc.Business.Implementation.Aggregation;

/// <summary>
/// Quand on a  :
///      //@Ecran(Ecran 1):Begin
///      //@Ecran(Ecran 1.1):Begin
///      //@Ecran(Ecran 1.1.1):Begin
///      //@Ecran(Ecran 1.1.1):End
///      //@Ecran(Ecran 1.1):End
///      //@Ecran(Ecran 1):End
///      
///      //@Ecran(Ecran 1):Begin
///      //@Ecran(Ecran 1.2):Begin
///      //@Ecran(Ecran 1.2.1):BeginAndEnd
///      //@Ecran(Ecran 1.2):End
///      //@Ecran(Ecran 1):End
///      
/// il retourne un groupage par rapport a Ecran 1 qui est commun
///      //@Ecran(Ecran 1):Begin
///      //@Ecran(Ecran 1.1):Begin
///      //@Ecran(Ecran 1.1.1):Begin
///      //@Ecran(Ecran 1.1.1):End
///      //@Ecran(Ecran 1.1):End
///      //@Ecran(Ecran 1.2.2):Begin
///      //@Ecran(Ecran 1.2.1):BeginAndEnd
///      //@Ecran(Ecran 1.2):End
///      //@Ecran(Ecran 1):End
///      
/// On ne peut avoir qu'un seul élément avec un nom et un type donné, 
/// on considère le premier trouvé,
/// si plusieurs occurences leurs enfants sont ajoutés à l'unique élément aggrégé
/// </summary>
public class CompositeAggregator : ICompositeAggregator
{
    public CompositeDefinition Aggregate(IEnumerable<CompositeDefinition> compositeDefinitions)
    {
        var newList = new CompositeCollection(null);
        var root = compositeDefinitions.FirstOrDefault();
        if(root is null)
        {
            return newList;
        }
        AggregateList(root, newList);
        return newList;
    }

    private void AggregateList(CompositeDefinition currentElement, CompositeCollection newList)
    {
        if (currentElement is CompositeElement element)
        {
            var existingElement = newList.Root.Search(element);
            if(existingElement is CompositeAggregation existingElementAsAggregation)
            {
                AddElementToExistingAggregation(element, existingElementAsAggregation);
            }
            else if (existingElement is CompositeElement existingElementAsElement)
            {
                var newAggregation = CreateAndAddGroupInNewAggregation(existingElementAsElement, newList);
                AddElementToExistingAggregation(element, newAggregation);
            }
            else
            {
                newList.Children.Add(element);
                element.Parent = newList;
            }
        }
        else if (currentElement is CompositeCollection collection)
        {
            if(collection.Children.Count==1 && collection.Children[0] is CompositeCollection collectionChild)
            {
                foreach (var elementTmp in collectionChild.Children.ToList())
                {
                    AggregateList(elementTmp, newList);
                }
            }
            else
            {
                var childCollection = new CompositeCollection(newList);
                newList.Children.Add(childCollection);
                foreach (var elementTmp in collection.Children)
                {
                    AggregateList(elementTmp, childCollection);
                }
            }
        }
    }

    private void AddElementToExistingAggregation(CompositeElement element, CompositeAggregation aggregation)
    {
        if (!element.HasChildren)
        {
            aggregation.Children.Add(element, new List<CompositeDefinition>());
        }
        else
        {
            var parentCollection = element.Parent as CompositeCollection;
            var childs = parentCollection.Children.Where(m => m.Id != element.Id).ToList();
            aggregation.Children.Add(element, childs);
        }
    }

    private CompositeAggregation CreateAndAddGroupInNewAggregation(CompositeElement element, CompositeCollection newList)
    {
        var newAggregation = new CompositeAggregation(element.ElementDetails, newList);
        var parentCollection = element.Parent as CompositeCollection;
        if (!element.HasChildren)
        {
            newAggregation.Children.Add(element, new List<CompositeDefinition>());
        }
        else
        {
            var childs = parentCollection.Children.Where(m => m.Id != element.Id).ToList();
            newAggregation.Children.Add(element, childs);
        }
        newList.Children.Add(newAggregation);
        newList.Children = newList.Children
            .Where(m=> !((m is CompositeCollection elementParent) 
                && elementParent.Children.Any(e => e.Id == element.Id)))
            .ToList();
        return newAggregation;
    }
}
