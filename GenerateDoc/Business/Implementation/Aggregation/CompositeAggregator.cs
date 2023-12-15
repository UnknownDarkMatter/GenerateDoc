using GenerateDoc.Business.Interfaces;
using GenerateDoc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        if(currentElement is CompositeElement element)
        {
            var existingElement = newList.Root.Search(element);
            if (existingElement != null)
            {
                var currentElementParentCollection = currentElement.Parent as CompositeCollection;
                if (existingElement is CompositeAggregation aggregation)
                {
                    aggregation.Children.Add(element, 
                        currentElementParentCollection.Children.Where(m=>m.Id != existingElement.Id).ToList());
                }
                else
                {
                    var newAggregation = new CompositeAggregation(element.ElementDetails, newList);
                    foreach (var child in currentElementParentCollection.Children)
                    {
                        if(child is CompositeElement aggregatedChildElement 
                            && aggregatedChildElement.ElementDetails.Equals(element.ElementDetails))
                        {
                            newAggregation.Children.Add(aggregatedChildElement,
                                ((CompositeCollection)aggregatedChildElement.Parent).Children
                                    .Where(m=> m.Id != aggregatedChildElement.Id
                                                && !(m is CompositeElement identicalChild 
                                                    && identicalChild.ElementDetails.Equals(aggregatedChildElement.ElementDetails)))
                                    .ToList());
                        }
                    }
                    newList.Children.Add(newAggregation);
                    newList.Children = newList.Children
                        .Where(m=> !(m is CompositeElement identicalChild
                                     && identicalChild.ElementDetails.Equals(element.ElementDetails)))
                        .ToList();
                }
            }
            else
            {
                newList.Children.Add(element);
            }
        }
        if(currentElement is CompositeCollection collection)
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
