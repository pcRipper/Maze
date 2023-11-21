using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Maze.Logic
{
    public static class Functions
    {
        public static Form getTopMDIParent(this Form current_form)
        {
            Form topMDIParent = current_form.MdiParent;

            while(topMDIParent != null && topMDIParent.MdiParent != null)
            {
                topMDIParent = topMDIParent.MdiParent;   
            }

            return topMDIParent;
        }

        public static bool doesBelongToRec(Pair<Pair<int,int>, Pair<int, int>> rectangle,Pair<int,int> point)
        {
            return
                rectangle.first.first <= point.first && point.first <= rectangle.second.first
                &&
                rectangle.first.second <= point.second && point.second <= rectangle.second.second
            ;
        }

        public static bool Collided(Pair<Pair<int, int>, Pair<int, int>> l, Pair<Pair<int, int>, Pair<int, int>> r)
        {
            return 
                Math.Min(l.second.second,r.second.second) >= Math.Max(l.first.second,r.first.second)
                &&
                Math.Min(l.second.first,r.second.first) >= Math.Max(l.first.first,r.first.first)
            ;
        }

        public static Pair<int,int> getCenter(Pair<Pair<int,int>, Pair<int, int>> rectangle)
        {
            return new(
                (rectangle.second.first - rectangle.first.first)/2 + rectangle.first.first, 
                (rectangle.second.second - rectangle.first.second)/2 + rectangle.first.second 
            );
        }

        public static void CopyBitmap(
            this Bitmap source, 
            ref Bitmap destination,
            Pair<Pair<int,int>,Pair<int,int>> copy_zone
        )
        {
            Pair<Pair<int, int>, Pair<int, int>> picture_rectangle = new(
                new(0,0), 
                new(source.Size.Height,source.Size.Width)
            );

            if (
                !doesBelongToRec(picture_rectangle, copy_zone.first)
                ||
                !doesBelongToRec(picture_rectangle, copy_zone.second)
            ) return;


            BitmapData sourceData = source.LockBits(
                new Rectangle(0, 0, source.Width, source.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb
            );

            try
            {
                BitmapData destinationData = destination.LockBits(
                    new Rectangle(0, 0, destination.Width, destination.Height),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format32bppArgb
                );

                try
                {
                    unsafe
                    {
                        byte* sourcePointer = (byte*)sourceData.Scan0;
                        byte* destinationPointer = (byte*)destinationData.Scan0;

                        for (int y = 0; y < sourceData.Height; y++)
                        {
                            for (int x = 0; x < sourceData.Width; x++)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    destinationPointer[i] = sourcePointer[i];
                                }
                                sourcePointer += 4;
                                destinationPointer += 4;
                            }

                            sourcePointer += sourceData.Stride - sourceData.Width * 4;
                            destinationPointer += destinationData.Stride - destinationData.Width * 4;
                        }
                    }
                }
                finally
                {
                    destination.UnlockBits(destinationData);
                }
            }
            finally
            {
                source.UnlockBits(sourceData);
            }
        }
    }
}
