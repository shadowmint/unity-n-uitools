

# RectTransform

## Terminology

![Terms](https://raw.github.com/shadowmint/unity-article-recttransform/master/docs/images/terms.png)

## Anchors

The first part of the rect transform to understand is the anchors.

The anchors define the size of the 'anchor area' for the layout, relative to its parent.

The four anchor points are defined by the `RectTransform` properties `anchorMin` and `anchorMax`:

    TopLeft = new Vector2(transform.anchorMin.x, transform.anchorMax.y)
    TopRight = new Vector2(transform.anchorMax.x, transform.anchorMax.y)
    BottomLeft = new Vector2(transform.anchorMin.x, transform.anchorMin.y)
    BottomRight = new Vector2(transform.anchorMax.x, transform.anchorMin.y)

They define a relative space inside the parent, creating a rectangle which is the 'full area' of the layout.

For example, if the bottom left point is (0.1, 0.1), and the top right point is (0.5, 0.8), then the
bounding rectangle is 40% of the width, and 70% of the height of the parent:

    |                         (1,1) <-- top corner of parent
    |
    |    |       (0.5, 0.8) <--- top corner of this component
    |    |
    |    | <-- 0.8 - 0.1 = 70% height of parent
    |    |
    |     -------  <-- 0.5 - 0.1 = 40% width of parent
    |
    |   (0.1, 0.1) <-- bottom corner of this component
    |
    (0,0) <-- bottom corner of parent
    
The anchors define the *porportional* bounding area of the component relative to it's parent.

However, we can also have a 'zero' proportial component if we want this child to be absolutely sized
with respect to it's parent. 

eg. If the height of the component should always be the same, but the width should scale with the
parent, we would set the top and bottom components to the same point:

     |                         (1,1) <-- top corner of parent
     |
     |    (0.1, 0.8)       (0.5, 0.8)  <-- The left and right values are will scale only.
     |
     (0,0) <-- bottom corner of parent
     
The defines the working area of the component as 40% of the width of the parent, and 0% of the height 
of the parent.

## Offsets

This leads to the offsets, which define the 'component area' (defined by the four blue offset points) that actually 
contains the layout for the component.

The offsets are basically a pair of offset values in *absolute canvas units* from the anchor area defined
by the anchors.

`maxOffset` is the distance of the top left corner of the area from the top level corner of the anchor area.
Notice this is in absolute units, so in order to be inside the area, the values here must be negative.

`minOffset` is the distance of the bottom left corner from the bottom left corner of the anchor area.

     |                         (1,1) <-- top corner of anchor area
     |
     |                (-5, -5)  <-- The top corner of the blue area, notice it is negative because it is left/down
     |
     |
     |    (5, 5) <-- The bottom corner of the blue area, notice it is +ve (up/right).     
     |
     (0,0) <-- bottom corner of anchor area
     
The offset units are canvas units; if the canvas is set to use pixels they would pixels; if its set to
use inches, then its inches.

Unlike the anchor coordinates, these are absolute units. You can calculate the size of the component area
using:

    widthFactor = anchor width proportion of parent
    heightFactor = anchor height proportion of parent
            
    Height = height of parent * heightFactor + transform.offsetMax.y - transform.offsetMin.y
    Width = width of parent * widthFactor + transform.offsetMax.x - transform.offsetMin.x

This leads to some significant outcomes:

If the heightFactor / widthFactor is zero, the size of the element is the same regardless of the
size of the parent. This is how the 'top left' preset works, for example.

If the offsetMin is too large, the object will have a *negative size*, as the width of the parent
becomes small; remember, these are absolute offsets.

So for example, if the distance from the top right anchor is one inch, and the anchors are set to expand 
to match the parent width, when the parent becomes small, the total width will become negative. 

If the size of the rendered area is negative in the x or y dimension, it will not render anything.

### Size delta

The `sizeDelta` property is simply:

    offsetMax - offsetMin

It's extremely important to understand that you shouldn't set this property!
    
The reason for this is complicated, but basically, it does not always do the right thing.

It might do almost the right thing sometimes... and the actual right thing sometimes...
but it won't do the right thing most of the time.

Consider, you have two numbers, A and B. You add the two numbers together to make a new value, C:

    C = A + B

This is a destructive operation. You cannot recover A and B from C, because there are any number of
possible (A,B) combinations that could have resulted in C.

Now, you set the value of C to 100. What should A be? What should B be?

Perhaps, if A was 1, and B was 2, and C was originally 3, then A should become 33, b 66? 

...but is that what you really wanted? 

Maybe A was 1 for a reason. Maybe the correct values of A and B should be 1 and 99.

The point is, setting `sizeDelta` is a destructive operation that forces the engine to **guess** what you
wanted the offset values to be. That guess will be wrong except in a very few cases.

For example, if you are using the 'top left' preset, with the pivot in the corner, and you set the size delta 
with current zero values for the offsets, setting the sizeDelta will expand the bottom right corner to the given
size and leave the top left corner in the correct (current) position.

In virtually every other situation it does the wrong thing.

This property should be read-only, and it is a fuckup of the API designers that it is not.

## Pivot and positioning

The `pivot` property defines a relative position from the bottom left corner of the component
area where the pivot is.

Typically this value will be either (0.5, 0.5) to center the pivot, or set to one of the corners of
the component area (0, 0), (1, 0), etc., because the values are porportional on the component area.

(0,0) is the bottom left corner of the component area, and (1, 1) is the top right.

The position of the rendered component is then generated by moving the component so that the pivot point
is set to the `anchoredPosition` offset from the 'anchor pivot', which is the relative offset into the
anchor area.

The value of the `anchoredPosition` is an *absolute offset* in canvas units, from the point in the
*anchor area* that matches the pivot.

Which is to say, if the pivot is say, (0.5, 0.5) then the offset is from the point (0.5, 0.5) in the
anchor area. This is the standard behaviour.

However, if you set the pivot to the top right, then the anchor pivot is the top right *of the anchor area*.
As such, an offset of say (-20px, -20px) moves the component down from the top right corner. 

...but if the pivot is the left middle, then the anchor pivot will be 50% up from the bottom of the anchor
area, and all the way on the left of the anchor area.

To repeat, because this is confusing... there are *two* pivots.

- The pivot point is given by using the `pivot` property on the rect transform as a relative offset into the *component area*.

- The anchor pivot point is given by using the `pivot` property on the rect transform as a relative offset into the *anchor area*.

The component is then positioned by moving the pivot point to be the absolute offset `anchoredPosition` from the anchor
pivot point.

### anchoredPosition3d

This is just the `Vector3` version of the `anchoredPosition`.


