kubectl get -n ewallet deploy -o yaml \
  | linkerd inject - \
  | kubectl apply -f -