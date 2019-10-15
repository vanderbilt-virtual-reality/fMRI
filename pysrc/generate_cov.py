import numpy as np

data = np.loadtxt("C:/Users/vedan/Documents/fMRI/data/TS.txt")
transpose_data = data.transpose()
covariance = np.cov(transpose_data)
print(covariance.shape)

