import numpy as np
import matplotlib.pyplot as plt
from CovManagement import *

data = np.loadtxt("../data/TS.txt")
transpose_data = data.transpose()
covariance = np.cov(transpose_data)
print(covariance.shape)
plt.imshow(covariance, cmap='hot', interpolation='nearest')
plt.show()

print(np.amax(covariance))
print(np.amin(covariance))
print(np.average(covariance))

threshold = np.percentile(covariance, 99)

tuples = cov_to_tuple(covariance, threshold)

cov_mats = ts_to_cov(data, 200)
np.savetxt("../data/tuples.csv", tuples, delimiter=',')

for mat in cov_mats:
    cur = dropout(mat, np.percentile(mat, 98.6))
    plt.imshow(cur, cmap='hot', interpolation='nearest')
    plt.show()

# print(cov_mats.shape)
# print("Threshold for 99th Percentile:\t", np.percentile(covariance, 99))
# print("\tNumber of Examples: ", np.count_nonzero(covariance > np.percentile(covariance, 99))/2)
# print("Threshold for 99.5th Percentile:\t", np.percentile(covariance, 99.5))
# print("\tNumber of Examples: ", np.count_nonzero(covariance > np.percentile(covariance, 99.5))/2)
# print("Threshold for 99.7th Percentile:\t", np.percentile(covariance, 99.7))
# print("\tNumber of Examples: ", np.count_nonzero(covariance > np.percentile(covariance, 99.5))/2)

# plt.hist(covariance.ravel(), bins='auto')
# plt.show()
# plt.imshow(covariance, cmap='hot', interpolation='nearest')
# plt.show()
# cd990 = dropout(covariance, np.percentile(covariance, 99))
# cd995 = dropout(covariance, np.percentile(covariance, 99.5))
# cd997 = dropout(covariance, np.percentile(covariance, 99.7))
# plt.imshow(cd990, cmap='hot', interpolation='nearest')
# plt.show()
# plt.imshow(cd995, cmap='hot', interpolation='nearest')
# plt.show()
# plt.imshow(cd997, cmap='hot', interpolation='nearest')
# plt.show()