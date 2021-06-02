using System;

namespace ThoughtWorks.QRCode.Codec.Ecc
{	
	public class ReedSolomon
	{
		//G(x)=a^8+a^4+a^3+a^2+1
		internal int[] _y;

		internal int[] _gexp = new int[512];
		internal int[] _glog = new int[256];
		internal int _NPAR;
		//final int NPAR = 15;
		internal int _MAXDEG;
		internal int[] _synBytes;

		/* The Error Locator Polynomial, also known as Lambda or Sigma. Lambda[0] == 1 */
		internal int[] _lambda;

		/* The Error Evaluator Polynomial */
		internal int[] _omega;

		/* local ANSI declarations */

		/* error locations found using Chien's search*/
		internal int[] _errorLocs = new int[256];
		internal int _nerrors;

		/* erasure flags */
		internal int[] _erasureLocs = new int[256];
		internal int _nerasures = 0;

		internal bool _correctionSucceeded = true;

		virtual public bool CorrectionSucceeded
		{
			get
			{
				return _correctionSucceeded;
			}
			
		}
		virtual public int NumCorrectedErrors
		{
			get
			{
				return _nerrors;
			}
			
		}
		
		public ReedSolomon(int[] source, int NPAR)
		{
			initializeGaloisTables();
			_y = source;
			this._NPAR = NPAR;
			_MAXDEG = NPAR * 2;
			_synBytes = new int[_MAXDEG];
			_lambda = new int[_MAXDEG];
			_omega = new int[_MAXDEG];
		}
		
		internal virtual void  initializeGaloisTables()
		{
			int i, z;
			int pinit, p1, p2, p3, p4, p5, p6, p7, p8;
			
			pinit = p2 = p3 = p4 = p5 = p6 = p7 = p8 = 0;
			p1 = 1;
			
			_gexp[0] = 1;
			_gexp[255] = _gexp[0];
			_glog[0] = 0; /* shouldn't log[0] be an error? */
			
			for (i = 1; i < 256; i++)
			{
				pinit = p8;
				p8 = p7;
				p7 = p6;
				p6 = p5;
				p5 = p4 ^ pinit;
				p4 = p3 ^ pinit;
				p3 = p2 ^ pinit;
				p2 = p1;
				p1 = pinit;
				_gexp[i] = p1 + p2 * 2 + p3 * 4 + p4 * 8 + p5 * 16 + p6 * 32 + p7 * 64 + p8 * 128;
				_gexp[i + 255] = _gexp[i];
			}
			
			for (i = 1; i < 256; i++)
			{
				for (z = 0; z < 256; z++)
				{
					if (_gexp[z] == i)
					{
						_glog[i] = z;
						break;
					}
				}
			}
		}
		
		/* multiplication using logarithms */
		internal virtual int gmult(int a, int b)
		{
			int i, j;
			if (a == 0 || b == 0)
				return (0);
			i = _glog[a];
			j = _glog[b];
			return (_gexp[i + j]);
		}
		
		
		internal virtual int ginv(int elt)
		{
			return (_gexp[255 - _glog[elt]]);
		}
		
		
		
		internal virtual void  decode_data(int[] data)
		{
			int i, j, sum;
			for (j = 0; j < _MAXDEG; j++)
			{
				sum = 0;
				for (i = 0; i < data.Length; i++)
				{
					sum = data[i] ^ gmult(_gexp[j + 1], sum);
				}
				_synBytes[j] = sum;
			}
		}
		
		public virtual void  correct()
		{
			//	
			decode_data(_y);
			_correctionSucceeded = true;
			bool hasError = false;
			for (int i = 0; i < _synBytes.Length; i++)
			{
				//Console.out.println("SyndromeS"+string.valueOf(i) + " = " + synBytes[i]);
				if (_synBytes[i] != 0)
					hasError = true;
			}
			if (hasError)
				_correctionSucceeded = correct_errors_erasures(_y, _y.Length, 0, new int[1]);
		}
		
		internal virtual void  Modified_Berlekamp_Massey()
		{
			int n, L, L2, k, d, i;
			int[] psi = new int[_MAXDEG];
			int[] psi2 = new int[_MAXDEG];
			int[] D = new int[_MAXDEG];
			int[] gamma = new int[_MAXDEG];
			
			/* initialize Gamma, the erasure locator polynomial */
			init_gamma(gamma);
			
			/* initialize to z */
			copy_poly(D, gamma);
			mul_z_poly(D);
			
			copy_poly(psi, gamma);
			k = - 1; L = _nerasures;
			
			for (n = _nerasures; n < 8; n++)
			{
				
				d = compute_discrepancy(psi, _synBytes, L, n);
				
				if (d != 0)
				{
					
					/* psi2 = psi - d*D */
					for (i = 0; i < _MAXDEG; i++)
						psi2[i] = psi[i] ^ gmult(d, D[i]);
					
					
					if (L < (n - k))
					{
						L2 = n - k;
						k = n - L;
						/* D = scale_poly(ginv(d), psi); */
						for (i = 0; i < _MAXDEG; i++)
							D[i] = gmult(psi[i], ginv(d));
						L = L2;
					}
					
					/* psi = psi2 */
					for (i = 0; i < _MAXDEG; i++)
						psi[i] = psi2[i];
				}
				
				mul_z_poly(D);
			}
			
			for (i = 0; i < _MAXDEG; i++)
				_lambda[i] = psi[i];
			compute_modified_omega();
		}
		
		/* given Psi (called Lambda in Modified_Berlekamp_Massey) and synBytes,
		compute the combined erasure/error evaluator polynomial as 
		Psi*S mod z^4
		*/
		internal virtual void  compute_modified_omega()
		{
			int i;
			int[] product = new int[_MAXDEG * 2];
			
			mult_polys(product, _lambda, _synBytes);
			zero_poly(_omega);
			for (i = 0; i < _NPAR; i++)
				_omega[i] = product[i];
		}
		
		/* polynomial multiplication */
		internal virtual void  mult_polys(int[] dst, int[] p1, int[] p2)
		{
			int i, j;
			int[] tmp1 = new int[_MAXDEG * 2];
			
			for (i = 0; i < (_MAXDEG * 2); i++)
				dst[i] = 0;
			
			for (i = 0; i < _MAXDEG; i++)
			{
				for (j = _MAXDEG; j < (_MAXDEG * 2); j++)
					tmp1[j] = 0;
				
				/* scale tmp1 by p1[i] */
				for (j = 0; j < _MAXDEG; j++)
					tmp1[j] = gmult(p2[j], p1[i]);
				/* and mult (shift) tmp1 right by i */
				for (j = (_MAXDEG * 2) - 1; j >= i; j--)
					tmp1[j] = tmp1[j - i];
				for (j = 0; j < i; j++)
					tmp1[j] = 0;
				
				/* add into partial product */
				for (j = 0; j < (_MAXDEG * 2); j++)
					dst[j] ^= tmp1[j];
			}
		}		
		
		/* gamma = product (1-z*a^Ij) for erasure locs Ij */
		internal virtual void  init_gamma(int[] gamma)
		{
			int e;
			int[] tmp = new int[_MAXDEG];
			
			zero_poly(gamma);
			zero_poly(tmp);
			gamma[0] = 1;
			
			for (e = 0; e < _nerasures; e++)
			{
				copy_poly(tmp, gamma);
				scale_poly(_gexp[_erasureLocs[e]], tmp);
				mul_z_poly(tmp);
				add_polys(gamma, tmp);
			}
		}
		
		
		
		internal virtual void  compute_next_omega(int d, int[] A, int[] dst, int[] src)
		{
			int i;
			for (i = 0; i < _MAXDEG; i++)
			{
				dst[i] = src[i] ^ gmult(d, A[i]);
			}
		}
		
		
		
		internal virtual int compute_discrepancy(int[] lambda, int[] S, int L, int n)
		{
			int i, sum = 0;
			
			for (i = 0; i <= L; i++)
				sum ^= gmult(lambda[i], S[n - i]);
			return (sum);
		}
		
		/// <summary>******* polynomial arithmetic ******************</summary>
		
		internal virtual void  add_polys(int[] dst, int[] src)
		{
			int i;
			for (i = 0; i < _MAXDEG; i++)
				dst[i] ^= src[i];
		}
		
		internal virtual void  copy_poly(int[] dst, int[] src)
		{
			int i;
			for (i = 0; i < _MAXDEG; i++)
				dst[i] = src[i];
		}
		
		internal virtual void  scale_poly(int k, int[] poly)
		{
			int i;
			for (i = 0; i < _MAXDEG; i++)
				poly[i] = gmult(k, poly[i]);
		}
		
		
		internal virtual void  zero_poly(int[] poly)
		{
			int i;
			for (i = 0; i < _MAXDEG; i++)
				poly[i] = 0;
		}
		
		
		/* multiply by z, i.e., shift right by 1 */
		internal virtual void  mul_z_poly(int[] src)
		{
			int i;
			for (i = _MAXDEG - 1; i > 0; i--)
				src[i] = src[i - 1];
			src[0] = 0;
		}
		
		
		/* Finds all the roots of an error-locator polynomial with coefficients
		* Lambda[j] by evaluating Lambda at successive values of alpha. 
		* 
		* This can be tested with the decoder's equations case.
		*/		
		internal virtual void  Find_Roots()
		{
			int sum, r, k;
			_nerrors = 0;
			
			for (r = 1; r < 256; r++)
			{
				sum = 0;
				/* evaluate lambda at r */
				for (k = 0; k < _NPAR + 1; k++)
				{
					sum ^= gmult(_gexp[(k * r) % 255], _lambda[k]);
				}
				if (sum == 0)
				{
					_errorLocs[_nerrors] = (255 - r); _nerrors++;
					//if (DEBUG) fprintf(stderr, "Root found at r = %d, (255-r) = %d\n", r, (255-r));
				}
			}
		}
		
		/* Combined Erasure And Error Magnitude Computation 
		* 
		* Pass in the codeword, its size in bytes, as well as
		* an array of any known erasure locations, along the number
		* of these erasures.
		* 
		* Evaluate Omega(actually Psi)/Lambda' at the roots
		* alpha^(-i) for error locs i. 
		*
		* Returns 1 if everything ok, or 0 if an out-of-bounds error is found
		*
		*/
		
		internal virtual bool correct_errors_erasures(int[] codeword, int csize, int nerasures, int[] erasures)
		{
			int r, i, j, err;
			
			/* If you want to take advantage of erasure correction, be sure to
			set NErasures and ErasureLocs[] with the locations of erasures. 
			*/
			_nerasures = nerasures;
			for (i = 0; i < _nerasures; i++)
				_erasureLocs[i] = erasures[i];
			
			Modified_Berlekamp_Massey();
			Find_Roots();
			
			
			if ((_nerrors <= _NPAR) || _nerrors > 0)
			{
				
				/* first check for illegal error locs */
				for (r = 0; r < _nerrors; r++)
				{
					if (_errorLocs[r] >= csize)
					{
						//if (DEBUG) fprintf(stderr, "Error loc i=%d outside of codeword length %d\n", i, csize);
						//Console.out.println("Error loc i="+ErrorLocs[r]+" outside of codeword length"+csize);
						return false;
					}
				}
				
				for (r = 0; r < _nerrors; r++)
				{
					int num, denom;
					i = _errorLocs[r];
					/* evaluate Omega at alpha^(-i) */
					
					num = 0;
					for (j = 0; j < _MAXDEG; j++)
						num ^= gmult(_omega[j], _gexp[((255 - i) * j) % 255]);
					
					/* evaluate Lambda' (derivative) at alpha^(-i) ; all odd powers disappear */
					denom = 0;
					for (j = 1; j < _MAXDEG; j += 2)
					{
						denom ^= gmult(_lambda[j], _gexp[((255 - i) * (j - 1)) % 255]);
					}
					
					err = gmult(num, ginv(denom));
					//if (DEBUG) fprintf(stderr, "Error magnitude %#x at loc %d\n", err, csize-i);
					
					codeword[csize - i - 1] ^= err;
				}
				//for (int p = 0; p < codeword.length; p++)
				//	Console.out.println(codeword[p]);
				//Console.out.println("correction succeeded");
				return true;
			}
			else
			{
				//if (DEBUG && NErrors) fprintf(stderr, "Uncorrectable codeword\n");
				//Console.out.println("Uncorrectable codeword");
				return false;
			}
		}
	}
}