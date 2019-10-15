import numpy as np
import matplotlib.pyplot as plt
import scipy as sc


def cov_to_tuple(X, thresh):
    toRet = []
    m = max(X.shape)
    for i in np.arange(0, m):
        for j in np.arange((i + 1), m):
            if (X(i, j) > thresh):
                toRet.append([i, j, X[i, j]])
    return np.asarray(toRet)

def ts_to_cov(X, frame):
    n = min(X.shape)
    t = max(X.shape)
    numMat = t - frame + 1
    toRet = np.zeros([numMat, n, n])
    for i in np.arange(0, numMat):
        cur = X[i:(i + frame), :]
        toRet[i] = np.cov(cur.T)
    return toRet