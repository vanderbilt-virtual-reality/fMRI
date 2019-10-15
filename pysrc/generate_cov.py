import numpy as np
import matplotlib.pyplot as plt
from pysrc.CovManagement import *

data = np.loadtxt("C:/Users/vedan/Documents/fMRI/data/TS.txt")
transpose_data = data.transpose()
covariance = np.cov(transpose_data)
print(covariance.shape)
plt.imshow(covariance, cmap='hot', interpolation='nearest')
plt.show()

print(np.amax(covariance))
print(np.amin(covariance))
print(np.average(covariance))

plt.hist(covariance.ravel(), bins='auto')
plt.show()
threshold = np.percentile(covariance, 99.5)

tuples = cov_to_tuple(covariance, threshold)

cov_mats = ts_to_cov(data, 1000)
for mat in cov_mats:
    mat = dropout(mat, np.percentile(mat, 99.5))
    plt.imshow(mat, cmap='hot', interpolation='nearest')
    plt.show()
print(cov_mats.shape)

