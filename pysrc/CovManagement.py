import numpy as np
import matplotlib.pyplot as plt
import scipy as sc


def cov_to_tuple(X, thresh):
    toRet = []
    m = max(X.shape)
    for i in np.arange(0, m):
        for j in np.arange((i + 1), m):
            if (X(i, j) > thresh):
                toRet.append(tuple([i, j]))
    return np.asarray(toRet)
