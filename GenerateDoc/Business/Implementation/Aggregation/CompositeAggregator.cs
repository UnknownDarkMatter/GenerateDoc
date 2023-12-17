using GenerateDoc.Business.Interfaces;
using GenerateDoc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        CompositeDefinition root;
        if(compositeDefinitions.Count() == 0)
        {
            root = new CompositeCollection(null);
        }
        else if(compositeDefinitions.Count() == 1)
        {
            root = compositeDefinitions.First();
        }
        else
        {
            var rootAsCollection = new CompositeCollection(null);
            root = rootAsCollection;
            rootAsCollection.Children.AddRange(compositeDefinitions);
            foreach (var element in compositeDefinitions)
            {
                element.Parent = rootAsCollection;
            }
        }
        var aggregatedComposite = new CompositeCollection(null);
        Aggregate(root, aggregatedComposite);
        return aggregatedComposite;
    }

    private void Aggregate(CompositeDefinition currentElement, CompositeCollection newList)
    {
        if (currentElement is CompositeElement element)
        {
            var existingElement = newList.Root.Search(element);
            if(existingElement is CompositeAggregation existingElementAsAggregation)
            {
                var elementChildren = GetChildren(element);
                AddElementToExistingAggregation(element, elementChildren, existingElementAsAggregation);
            }
            else if (existingElement is CompositeElement existingElementAsElement)
            {
                var existingElementChildren = GetChildren(existingElementAsElement);
                var newAggregation = CreateAndAddGroupInNewAggregation(existingElementAsElement, existingElementChildren, newList);
                if(existingElementAsElement != element)
                {
                    var elementChildren = GetChildren(element);
                    AddElementToExistingAggregation(element, elementChildren, newAggregation);
                }
            }
            else
            {
                newList.Children.Add(element);
                element.Parent = newList;
            }
        }
        else if (currentElement is CompositeCollection collection)
        {
            AddCollectionChildsRecursively(collection, newList);
        }
    }

    private void AddElementToExistingAggregation(CompositeElement element, List<CompositeDefinition> elementChildren,
        CompositeAggregation aggregation)
    {
        element.IsInsideAggregationGroup = true;
        element.Parent = aggregation;
        if (!element.HasChildren)
        {
            aggregation.Children.Add(element, new List<CompositeDefinition>());
        }
        else
        {
            aggregation.Children.Add(element, elementChildren);
        }
    }

    private CompositeAggregation CreateAndAddGroupInNewAggregation(CompositeElement element, 
        List<CompositeDefinition> elementChildren, CompositeCollection newList)
    {
        var parentCollection = element.Parent as CompositeCollection;
        var newAggregation = new CompositeAggregation(element.ElementDetails, parentCollection);
        element.IsInsideAggregationGroup = true;
        element.Parent = newAggregation;
        if (!element.HasChildren)
        {
            newAggregation.Children.Add(element, new List<CompositeDefinition>());
        }
        else
        {
            newAggregation.Children.Add(element, elementChildren);
        }

        var parentChildren = (parentCollection.Parent as CompositeCollection).Children;
        parentChildren.Remove(parentCollection);
        parentChildren.Add(newAggregation);
        parentChildren = parentChildren.Where(m => (!(m is CompositeCollection)) 
            || (m is CompositeCollection c && c.Children.Any())).ToList();
        (newList.Parent as CompositeCollection).Children.Remove(newList);
        (newList.Parent as CompositeCollection).Children.AddRange(parentChildren);

        return newAggregation;
    }

    private List<CompositeDefinition> GetChildren(CompositeElement element)
    {
        if (!element.HasChildren) { return new List<CompositeDefinition>();  }

        var children = (element.Parent as CompositeCollection).Children;
        children = children.Where(m => m.Id != element.Id).ToList();
        return children;
    }

    private void AddCollectionChildsRecursively(CompositeCollection collection, CompositeCollection newList)
    {
        if (collection.Children.Count == 1
            && collection.Children[0] is CompositeCollection collectionChild
            && collectionChild.Children.Count == 1)
        {
            foreach (var elementTmp in collectionChild.Children.ToList())
            {
                Aggregate(elementTmp, newList);
            }
        }
        else
        {
            var childCollection = new CompositeCollection(newList);
            newList.Children.Add(childCollection);
            var childrenToAdd = collection.Children;
            if (childrenToAdd.Count == 1
                && childrenToAdd[0] is CompositeCollection collectionToAddChild)
            {
                childrenToAdd = collectionToAddChild.Children;
            }
            foreach (var elementTmp in childrenToAdd.ToList())
            {
                Aggregate(elementTmp, childCollection);
            }
        }
    }

    private bool IsCollectionOfElementWithChilds(List<CompositeDefinition> collection, out CompositeElement elementWithChilds)
    {
        elementWithChilds = collection.FirstOrDefault(m => (m is CompositeElement e) && e.HasChildren) as CompositeElement;
        return elementWithChilds != null;
    }

}
