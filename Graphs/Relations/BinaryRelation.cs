
using System.Diagnostics.CodeAnalysis;

namespace Graphs.Relations;

public sealed class BinaryRelation<TPreimage, TImage> where TPreimage : notnull where TImage : notnull
{
    private readonly Dictionary<TPreimage, HashSet<TImage>> imageByPreimage;
    private readonly Dictionary<TImage, HashSet<TPreimage>> preimageByImage;

    public BinaryRelation()
    {
        imageByPreimage = new();
        preimageByImage = new();
    }

    public BinaryRelation(HashSet<TPreimage> preimageSet, HashSet<TImage> imageSet, Func<TPreimage, TImage, bool> predicate) : this()
    {
        foreach(TPreimage preimage in preimageSet)
        {
            foreach(TImage image in imageSet)
            {
                if(predicate.Invoke(preimage, image))
                {
                    Add(preimage, image);
                }
            }
        }
    }

    public void Add(TPreimage preimage, TImage image)
    {
        if(imageByPreimage.TryGetValue(preimage, out HashSet<TImage>? imageSet))
        {
            imageSet.Add(image);
        }
        else
        {
            imageByPreimage.Add(preimage, new() { image });
        }

        if(preimageByImage.TryGetValue(image, out HashSet<TPreimage>? preimageSet))
        {
            preimageSet.Add(preimage);
        }
        else
        {
            preimageByImage.Add(image, new() { preimage });
        }
    }
    
    public void Clear()
    {
        imageByPreimage.Clear();
        preimageByImage.Clear();
    }
    
    public bool ContainsPreimage(TPreimage preimage)
    {
        return imageByPreimage.ContainsKey(preimage);
    }

    public bool ContainsImage(TImage image)
    {
        return preimageByImage.ContainsKey(image);
    }

    public bool Remove(TPreimage preimage, TImage image)
    {
        if(imageByPreimage.TryGetValue(preimage, out HashSet<TImage>? imageSet) &&
            preimageByImage.TryGetValue(image, out HashSet<TPreimage>? preimageSet))
        {
            imageSet.Remove(image);
            preimageSet.Remove(preimage);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TryGetPreimage(TImage image, [MaybeNullWhen(false)] out HashSet<TPreimage> result)
    {
        return preimageByImage.TryGetValue(image, out result);
    }

    public bool TryGetImage(TPreimage preimage, [MaybeNullWhen(false)] out HashSet<TImage> result)
    {
        return imageByPreimage.TryGetValue(preimage, out result);
    }

}