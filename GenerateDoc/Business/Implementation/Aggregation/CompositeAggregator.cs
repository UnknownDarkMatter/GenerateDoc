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
        Aggregate(root, aggregatedComposite, null);
        return aggregatedComposite;
    }

    private void Aggregate(CompositeDefinition currentElement, CompositeDefinition newList, CompositeElement aggregatedElementKey)
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
                CompositeAggregation newAggregation = newList.Root.Search(existingElementAsElement)?.Parent as CompositeAggregation;
                if(newAggregation is null)
                {
                    newAggregation = CreateAndAddGroupInNewAggregation(existingElementAsElement, newList);
                }
                if (existingElementAsElement != element)
                {
                    AddElementToExistingAggregation(element, newAggregation);
                }
            }
            else
            {
                if(newList is CompositeCollection c)
                {
                    c.Children.Add(element);
                }
                else if(newList is CompositeAggregation a)
                {
                    a.Children[aggregatedElementKey].Add(element);
                }
                element.Parent = newList;
            }
        }
        else if (currentElement is CompositeCollection collection)
        {
            AddCollectionChildsRecursively(collection, newList, aggregatedElementKey);
        }
    }

    private void AddElementToExistingAggregation(CompositeElement element, CompositeAggregation aggregation)
    {
        element.IsInsideAggregationGroup = true;
        if (!aggregation.Children.ContainsKey(element))
        {
            aggregation.Children.Add(element, new List<CompositeDefinition>());
        }
        if (element.HasChildren)
        {
            var children = new List<CompositeDefinition>();
            if (element.Parent is CompositeCollection c)
            {
                children = c.Children;
            }
            else if (element.Parent is CompositeAggregation a)
            {
                children = a.Children[element];
            }

            children = children.Where(m => m.Id != element.Id).ToList();

            foreach (var child in children.ToList())
            {
                if (child is CompositeElement e)
                {
                    var existingChildren = element.Root.Search(e);
                    if (existingChildren != null)
                    {
                        continue;
                    }
                }
                Aggregate(child, aggregation, element);
            }
        }
        element.Parent = aggregation;
    }

    private CompositeAggregation CreateAndAddGroupInNewAggregation(CompositeElement element,
        CompositeDefinition newList)
    {
        var newAggregation = new CompositeAggregation(element.ElementDetails, element.Parent);
        element.IsInsideAggregationGroup = true;
        if (!element.HasChildren)
        {
            newAggregation.Children.Add(element, new List<CompositeDefinition>());

            if (element.Parent is CompositeCollection parentCollection)
            {
                if(parentCollection.Parent is CompositeCollection parentParentCollection)
                {
                    var parentChildren = parentParentCollection.Children;
                    parentChildren.Remove(parentCollection);
                    parentChildren.Add(newAggregation);
                    parentChildren = parentChildren.Where(m => (!(m is CompositeCollection))
                        || (m is CompositeCollection c && c.Children.Any())).ToList();

                }
                else if (parentCollection.Parent is CompositeAggregation parentParentAggregation)
                {
                    var parentOfElement = newList.Root.Search(element).Parent;
                    if(parentOfElement is CompositeCollection parentCollectionForAggregation)
                    {
                        parentCollectionForAggregation.Children.Remove(element);
                        parentCollectionForAggregation.Children.Add(newAggregation);
                    }
                }

                if (newList.Parent is CompositeCollection c)
                {
                    c.Children.Remove(newList);
                }
            }
            else if (element.Parent is CompositeAggregation parentAggregation)
            {
                throw new NotImplementedException();
            }

        }
        else
        {
            var children = new List<CompositeDefinition>();
            if (element.Parent is CompositeCollection c)
            {
                children = c.Children;
            } 
            else if(element.Parent is CompositeAggregation a)
            {
                children = a.Children[element];
            }

            if (element.Parent is CompositeCollection parentCollection)
            {
                var parentChildren = (parentCollection.Parent as CompositeCollection).Children;
                parentChildren.Remove(parentCollection);
                parentChildren.Add(newAggregation);
                parentChildren = parentChildren.Where(m => (!(m is CompositeCollection))
                    || (m is CompositeCollection c && c.Children.Any())).ToList();

                if (newList.Parent is CompositeCollection c2)
                {
                    c2.Children.Remove(newList);
                }
            }
            else if (element.Parent is CompositeAggregation parentAggregation)
            {
                throw new NotImplementedException();
            }

            foreach (var child in children.ToList())
            {
                if(child is CompositeElement e)
                {
                    var existingChildren = element.Root.Search(e);
                    if(existingChildren != null)
                    {
                        continue;
                    }
                }
                Aggregate(child, newAggregation, element);
            }
        }
        element.Parent = newAggregation;

        return newAggregation;
    }

    private void AddCollectionChildsRecursively(CompositeCollection collection, CompositeDefinition newList,
        CompositeElement aggregatedElementKey)
    {
        if (collection.Children.Count == 1
            && collection.Children[0] is CompositeCollection collectionChild
            && collectionChild.Children.Count == 1)
        {
            foreach (var elementTmp in collectionChild.Children.ToList())
            {
                Aggregate(elementTmp, newList, aggregatedElementKey);
            }
        }
        else
        {
            var childCollection = new CompositeCollection(newList);
            if(newList is CompositeCollection c)
            {
                c.Children.Add(childCollection);
            }
            else if (newList is CompositeAggregation a)
            {
                if (!a.Children.ContainsKey(aggregatedElementKey))
                {
                    a.Children.Add(aggregatedElementKey, new List<CompositeDefinition>());
                }
                a.Children[aggregatedElementKey].Add(childCollection);
            }
            var childrenToAdd = collection.Children;
            if (childrenToAdd.Count == 1
                && childrenToAdd[0] is CompositeCollection collectionToAddChild)
            {
                childrenToAdd = collectionToAddChild.Children;
            }
            foreach (var elementTmp in childrenToAdd.ToList())
            {
                Aggregate(elementTmp, childCollection, aggregatedElementKey);
            }
        }
    }


}
